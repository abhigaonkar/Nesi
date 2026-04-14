using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Common.Cache;
using NESI.BLL.Common.Shared;
using File = NESI.DTO.ViewModels.Core.FileManager.File;
using TreeNode = NESI.DTO.ViewModels.Core.TreeNode;

namespace NESI.BLL.Core.FileManager
{
	public abstract class NeFileBase : BLLBase
	{
	    public string FileServer => Path.Combine(Common.Shared.Configuration.UNCBasePath, "nesi_files");

	    public abstract string BaseFolder { get;  }

		public abstract string BasePath { get; }

		public virtual void Validate_folder_contents()
		{
			CreateFolder();
		}

		public virtual void CreateFolder()
		{
			Verify_folders_exist(BasePath);
		}

		protected NeFileBase()
		{
			
		}
		protected NeFileBase(Employee.Employee user) : base(user)
		{
			
		}

		public static string Clean_filename(string filename)
		{
			filename = filename.Trim();
			var illegals = Path.GetInvalidFileNameChars();
			return illegals.Aggregate(filename, (current, c) => current.Replace(c.ToString(), "")).TrimEnd('.');
		}

		public virtual string GetBaseFolder(int buId, bool externalUse)
		{
			var bu = Global.BusinessUnit.GetValue(buId);
			var teId = bu.tax_entity_id;
			// CheckEntityFolderStructure(teId);
			//JA the folder locations should come from appsettings...
			return externalUse
				? $@"/nesi_files/TE/TE{teId}"
				: $@"{FileServer}\nesi_files\TE\TE{teId}";
		}

		public void CheckEntityFolderStructure(int _teId)
		{
			var baseFolder = FileServer + @"\TE";
			var teBaseFolder = $@"{baseFolder}\TE{_teId}";
			var bu_s = _db.Database.SqlQuery<string>(
				@"Select GROUP_CONCAT(CONCAT('BU',id)) 
					from business_unit where tax_entity_id = @v0", _teId).FirstOrDefault();
			if (bu_s == null) return;
			bu_s = bu_s.Replace(',', '|');

			if (!Directory.Exists(baseFolder)) return;
			if (!Directory.Exists(teBaseFolder))
			{
				Directory.CreateDirectory(teBaseFolder);
			}

			var subFolderList = new[]
			{
				"_protected|NesiFileManager-InternalPublic|NesiFileManager-Confidential|NesiFileManager-WideOpenPublic",
				"applicant_files",
				"asset_pics",
				"business_unit_files|" + bu_s,
				"credit_card_receipts",
				"customer_asset_files",
				"customer_files",
				"discipline_files",
				"ERItemFolders",
				"Tax_Entity_Files|YearEnd|MonthEnd|Capital_Assets|BVBackup|Cash_Management|Legal|Insurance|Policies|SRED",
				"ftp",
				"inventory_files",
				"member_files",
				"MessageboardImages",
				"pos",
				"ProjectFolders",
				"quote_store",
				"quote_worksheet_files",
				"safety_files",
				"task_files",
				"training_documents",
				"training_files",
				"TrainingVideos",
				"vendor",
				"videos",
				"wos",
                "signature_files"
			};
			foreach (var subFolder in subFolderList)
			{
				var childFolders = subFolder.Contains("|")
					? subFolder.Split('|').Skip(1)
						.ToArray() // Doing this because we don't want the first folder to be part of the sub set
					: new string[] { };
				var topLevelSubFolder = childFolders.Any() ? subFolder.Split('|')[0] : subFolder;
				var teSubFolder = $@"{teBaseFolder}\{topLevelSubFolder}";
				if (!Directory.Exists(teSubFolder))
				{
					Directory.CreateDirectory(teSubFolder);
				}
				if (!childFolders.Any()) continue;
				foreach (var childFolder in childFolders)
				{
					var teSubChildFolder = $@"{teSubFolder}\{childFolder}";
					if (!Directory.Exists(teSubChildFolder))
					{
						Directory.CreateDirectory(teSubChildFolder);
					}
				}
			}
		}

		protected void Verify_folders_exist(string _subpath, params string[] _folders)
		{
			try
			{
				Verify_folder_exists(_subpath);
				foreach (var f in _folders)
				{
					Verify_folder_exists(_subpath + @"\" + f);
				}
			}
			catch (Exception)
			{
				// ignored
			}
		}

		protected void Verify_folder_exists(string _folder)
		{
			try
			{
				if (_folder.StartsWith(@"\f:\"))
				{
					_folder = _folder.TrimStart('\\');
				}
				if (!Directory.Exists(_folder))
				{
					Directory.CreateDirectory(_folder);
				}
			}
			catch (Exception)
			{
				// ignored
			}
		}

		public DTO.ViewModels.Core.FileManager.Directory GetDirectoryInfo(string path)
		{
			var stack = new Stack<DTO.ViewModels.Core.FileManager.Directory>();
			var rootDirectory = new DirectoryInfo(path);
			var node = new DTO.ViewModels.Core.FileManager.Directory()
			{
				Name = rootDirectory.Name,
				FullName = rootDirectory.FullName,
				SubDirectories = new List<DTO.ViewModels.Core.FileManager.Directory>(),
				Files = new List<File>()

			};
			stack.Push(node);
			while (stack.Count > 0)
			{
				var currentNode = stack.Pop();

				var directoryInfo = new DirectoryInfo(currentNode.FullName);
				if(!directoryInfo.Exists) continue;
				foreach (var directory in directoryInfo.GetDirectories())
				{
					var childDirectoryNode = new DTO.ViewModels.Core.FileManager.Directory()
					{
						Name = directory.Name,
						FullName = directory.FullName,
						SubDirectories = new List<DTO.ViewModels.Core.FileManager.Directory>(),
						Files = new List<File>()
					};
					currentNode.SubDirectories.Add(childDirectoryNode);
					stack.Push(childDirectoryNode);
				}
				foreach (var file in directoryInfo.GetFiles("*.*"))
					currentNode.Files.Add(new DTO.ViewModels.Core.FileManager.File()
					{
						Name = file.Name,
						FullName = file.FullName,
						LastModified = file.LastWriteTime,
						Size = file.Length,
						Extension = file.Extension,
						MimeType = BLL.Common.Shared.MimeTypeMap.GetMimeType(file.Extension)
					}
					);
			}
			return node;
		}

		public DTO.ViewModels.Core.FileManager.File[] GetFiles(string url)
		{
			var dir = GetDirectoryInfo(url);
			return dir.Files.ToArray();
		}

		public DTO.ViewModels.Core.FileManager.Directory GetDirectory()
		{
			return GetDirectoryInfo(BasePath);
		}
		public TreeNode[] GetTreeNode()
		{
			return GetDirectoryTreeNode(BasePath);
		}
		public TreeNode[] GetDirectoryTreeNode(string path)
		{
			var stack = new Stack<TreeNode>();
			var rootDirectory = new DirectoryInfo(path);
			var node = new TreeNode()
			{
				Label = rootDirectory.Name,
				Data = rootDirectory.FullName,
				Children = new List<TreeNode>(),
				ExpandedIcon = "fa-folder-open",
				CollapsedIcon = "fa-folder"
				
			};
			stack.Push(node);
			while (stack.Count > 0)
			{
				var currentNode = stack.Pop();

				var directoryInfo = new DirectoryInfo(currentNode.Data);
				if(!directoryInfo.Exists) continue;
				foreach (var directory in directoryInfo.GetDirectories())
				{
					var childDirectoryNode = new TreeNode()
					{
						Label = directory.Name,
						Data = directory.FullName,
						ExpandedIcon = "fa-folder-open-o",
						CollapsedIcon = "fa-folder",
						Children = new List<TreeNode>()
					};
					currentNode.Children.Add(childDirectoryNode);
					stack.Push(childDirectoryNode);
				}
				//foreach (var file in directoryInfo.GetFiles("*.*"))
				//	currentNode.Children.Add(new DTO.ViewModels.Core.FileManager.TreeNode()
				//	{
				//		Label = file.Name,
				//		Data = file.FullName,
				//		Icon = "fa-file-o",
				//	}
				//	);
				if (currentNode.Children.Count == 0)
				{
					currentNode.Children = null;
					currentNode.ExpandedIcon = null;
					currentNode.CollapsedIcon = null;
					currentNode.Icon = "fa-folder";
				}
			}
			return new List<TreeNode> { node }.ToArray();

		}

	}
}
Param([String] $path = "", [String]$old = "99", [String]$new = "99")
if($path -eq "" -AND $old -eq "99" -AND $new -eq "99")
{
echo "Usage: changeoutdlls.ps1 {bin path} {old version [ex: 17.1]} {new version [ex: 17.2]}"
exit
}

if($path -eq "")
{
echo "Please enter the path to your bin folder"
exit
}
if($old -eq "99")
{
echo "Please enter the old version of DevEx"
exit
}
if($new -eq "99")
{
echo "Please enter the new version of DevEx"
exit
}

if(!(Test-Path "C:\Program Files (x86)\DevExpress $new\Components\Bin\Framework\"))
{
echo "This version of DevEx isn't installed"
exit
}
#echo "Part 1"
svn revert -q $path -R
svn cleanup -q $path --remove-unversioned
#echo "Part 2"	
$files = Get-ChildItem "C:\Repository\trunk\nesi\Bin\Dev*$old*.dll"
$NoExistCount = 0
#Existence check
#echo "Part 3"
for ($i=0; $i -lt $files.Count; $i++) {
    $oldname = $files[$i].Name
	$newname = $oldname.replace($old, $new)
	$fullpath = $files[$i].FullName
	if(!(Test-Path "C:\Program Files (x86)\DevExpress $new\Components\Bin\Framework\$newname"))
		{
		$NoExistCount++
		}
    #echo $oldname+" => "$newname
}
#echo "Part 4"
if($NoExistCount -gt 0)
	{
	echo "There are old DevEx files that don't exist in the new version, can't proceed automatically"
	exit
	}
else
	{
	#Check if Temp folders exists
	if(!(Test-Path "C:\temp\"))
		{
		md "C:\temp\" 
		}
	if(!(Test-Path "c:\temp\old_devex_files\"))
		{
		md "C:\temp\old_devex_files\"
		}
	#echo "Part 5"
	$files = Get-ChildItem "C:\Repository\trunk\nesi\Bin\Dev*$old*.dll"
	for ($i=0; $i -lt $files.Count; $i++) 
		{
		$oldname = $files[$i].Name
		$newname = $oldname.replace($old, $new).replace("dll", "*")
		$newpath = "C:\Program Files (x86)\DevExpress $new\Components\Bin\Framework\$newname"
		
		$old_fullpath_dll = $files[$i].FullName
		$old_fullpath_xml = $files[$i].FullName.replace("dll", "xml")
		$new_fullpath_dll = $files[$i].FullName.replace($old, $new)
		$new_fullpath_xml = $files[$i].FullName.replace($old, $new).replace("dll", "xml")
		
		mv $old_fullpath_dll "C:\temp\old_devex_files\" -Force
		svn delete -q $old_fullpath_dll
		if(Test-Path $old_fullpath_xml)
			{
			mv $old_fullpath_xml "C:\temp\old_devex_files\" -Force 
			svn delete -q $old_fullpath_xml
			}
		cp -Force $newpath $path
		#echo $oldname+" => "$newname
		}
	#echo "Part 6"
	svn add -q "$path\DevExpress.*"
	echo "Copied all DevEx files to your Bin folder, and added them to versioning in SVN, you will need to commit these yourself"
	}

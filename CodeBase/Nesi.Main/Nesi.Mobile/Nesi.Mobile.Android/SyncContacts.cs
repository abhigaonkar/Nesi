using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using Android.Database;
using Android.Provider;
using Nesi.Mobile.Droid;
using Nesi.Mobile.Models;
using Xamarin.Forms;
using static Android.Provider.ContactsContract;

[assembly: Dependency(typeof(SyncContacts))]
namespace Nesi.Mobile.Droid
{
    class SyncContacts : Java.Lang.Object, IContacts
    {
       
        public async Task AddContacts(string FirstName, string LastName, string PhoneNumber, string Email, string CompanyName)
        {



            List<ContentProviderOperation> ops = new List<ContentProviderOperation>();
            int rawContactInsertIndex = ops.Count;

            ContentProviderOperation.Builder builder =
                ContentProviderOperation.NewInsert(ContactsContract.RawContacts.ContentUri);
            builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountType, null);
            builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountName, null);
            ops.Add(builder.Build());

            //Name
            builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
            builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, rawContactInsertIndex);
            builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                ContactsContract.CommonDataKinds.StructuredName.ContentItemType);
            builder.WithValue(ContactsContract.CommonDataKinds.StructuredName.FamilyName, LastName);
            builder.WithValue(ContactsContract.CommonDataKinds.StructuredName.GivenName, FirstName);
            ops.Add(builder.Build());


            //Number
            builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
            builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, rawContactInsertIndex);
            builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                ContactsContract.CommonDataKinds.Phone.ContentItemType);
            builder.WithValue(ContactsContract.CommonDataKinds.Phone.Number, PhoneNumber);
            builder.WithValue(ContactsContract.CommonDataKinds.StructuredPostal.InterfaceConsts.Type,
                    ContactsContract.CommonDataKinds.StructuredPostal.InterfaceConsts.TypeCustom);
            builder.WithValue(ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Label, "Primary Phone");
            ops.Add(builder.Build());

            //Email
            builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
            builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, rawContactInsertIndex);
            builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                ContactsContract.CommonDataKinds.Email.ContentItemType);
            builder.WithValue(ContactsContract.CommonDataKinds.Email.InterfaceConsts.Data, Email);
            builder.WithValue(ContactsContract.CommonDataKinds.Email.InterfaceConsts.Type,
                ContactsContract.CommonDataKinds.Email.InterfaceConsts.TypeCustom);
            builder.WithValue(ContactsContract.CommonDataKinds.Email.InterfaceConsts.Label, "Email");
            ops.Add(builder.Build());

            //Organization
            builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
            builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, rawContactInsertIndex);
            builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                ContactsContract.CommonDataKinds.Organization.ContentItemType);
            builder.WithValue(ContactsContract.CommonDataKinds.Organization.InterfaceConsts.Data, CompanyName);
            builder.WithValue(ContactsContract.CommonDataKinds.Organization.InterfaceConsts.Type,
                ContactsContract.CommonDataKinds.Organization.InterfaceConsts.TypeCustom);
            builder.WithValue(ContactsContract.CommonDataKinds.Organization.InterfaceConsts.Label, "Work");
            ops.Add(builder.Build());



            try
            {

                Android.App.Application.Context.ContentResolver.ApplyBatch(ContactsContract.Authority, ops);
                

            }

            catch (Exception e)
            {
                var ex = e.Message;
                

            }


        }

        public async Task DeleteContacts(string FirstName, string LastName, string PhoneNumber, string Email, string CompanyName)
        {

            Context thisContext = Android.App.Application.Context;


            string[] Projection = new string[] { ContactsContract.ContactsColumns.LookupKey, ContactsColumns.DisplayName };

            ICursor cursor = thisContext.ContentResolver.Query(ContactsContract.Contacts.ContentUri, Projection, null, null, null);

            while (cursor != null & cursor.MoveToNext())
            {
                string lookupKey = cursor.GetString(0);
                string name = cursor.GetString(1);
               
                if (name == FirstName + " " + LastName.Trim())
                {
                    var uri = Android.Net.Uri.WithAppendedPath(ContactsContract.Contacts.ContentLookupUri, lookupKey);
                    thisContext.ContentResolver.Delete(uri, null, null);
                    cursor.Close();
                    return;
                }
            }




        }


    }
}
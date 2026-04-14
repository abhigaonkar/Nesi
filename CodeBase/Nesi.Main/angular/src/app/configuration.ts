import { LoggingErrorHandlerOptions, ErrorOutputOptions } from './core/errorhandler/loggingErrorHandlerOptions';
import { ResponsiveConfig } from 'ng2-responsive';
import { NesiMenuType } from 'app/models/layout/nesiMenuItem';

export function HostContains(name: string) {
  return (window.location.hostname + ':' + window.location.port).toLowerCase().indexOf(name.toLowerCase()) >= 0
}
export let prevLog = { msg: '', obj: '' };

// Global constants
export let CONFIG = {
  ISDEBUG: () => HostContains(':430'),
  ISDEV: () => !HostContains(':6') &&
    (HostContains('localhost') || HostContains('devbeta') || HostContains('alpha') || HostContains('luke')),
  ISBETA: () => HostContains('beta') && !HostContains('devbeta'),
  ISALPHA: () => HostContains('alpha') || HostContains('luke'),
  ISLIVE: () => window.location.hostname.toLowerCase() === 'www.nesi.ca' || window.location.hostname.toLowerCase() === 'nesi.ca',
  ISDEVELOPER_ONLY: () => HostContains('devbeta'),

  // redirect to angular page with these page id in beta sites.
  betaMenus: [
    // {
    //   id: 120,
    //   routerLink: '/home/51/reports/120/master_contacts',
    // },
    // {
    //   id: 126,
    //   routerLink: '/home/51/reports/126/customer_assets',
    // },
    // {
    //   id: 211,
    //   routerLink: '/home/51/reports/211/master_vendors',
    // },
    {
      id: 141,
      routerLink: '/home/51/reports/141/cust_status_history',
      dev_beta_only: true,
      hide_badge: false,
    },
    {
      id: 203,
      routerLink: '/home/51/reports/203/master_phone_call_grid',
      dev_beta_only: true,
      hide_badge: false,
    },
    {
      id: 150,
      routerLink: '/home/51/reports/150/master_responsibilities_grid',
      dev_beta_only: true,
      hide_badge: false,
    },
    {
      id: 196,
      routerLink: '/home/51/reports/196/partner_specialties',
      dev_beta_only: true,
      hide_badge: false,
    },
    {
      id: 105,
      routerLink: '/home/51/reports/105/wo_line_grid',
      dev_beta_only: true,
      hide_badge: false,
    },
    {
      id: 102,
      routerLink: '/home/51/reports/102/master_purchases_grid',
      dev_beta_only: true,
      hide_badge: false,
    },
    {
      id: 113,
      routerLink: '/home/51/reports/113/master_outstanding_invoicesGrid',
      dev_beta_only: true,
      hide_badge: false,
    },
  ],
  // redirect to angular page with these page id in all sites.
  releaseMenus: [
    {
      id: 12,
      routerLink: '/home/12/workorder',
      mobileN1Link: true,
      mapN1Link: 'wo_prog.aspx',
      hide_badge: true,
    },
    {
      id: 92,
      routerLink: '/home/92/purchaseorder',
      mobileN1Link: false,
      mapN1Link: 'po_prog.aspx',
      hide_badge: true,
    },
    {
      id: 11,
      routerLink: '/home/11/vendors',
      //dev_beta_only: false,
      hide_badge: true,
    },
    {
      id: 77,
      routerLink: '/home/51/reports/77/master_workorder',
      dev_beta_only: false,
    },
    {
      id: 193,
      routerLink: '/home/51/reports/193/master_workorder2',
      dev_beta_only: false,
    },
    {
      id: 138,
      routerLink: '/home/138/applicants',
      dev_beta_only: false,
      hide_badge: true,
    },
    {
      id: 127,
      routerLink: '/home/127/employees',
      dev_beta_only: false,
      hide_badge: true,
    },
    {
      id: 219,
      routerLink: '/home/51/reports/219/advance_pay_report',
      dev_beta_only: false,
    },
    {
      id: 157,
      routerLink: '/home/51/reports/157/customer_login_history',
      dev_beta_only: false,
    },
    {
      id: 125,
      routerLink: '/home/51/reports/125/customer_rates_grid',
      dev_beta_only: false,
    },
    {
      id: 161,
      routerLink: '/home/51/reports/161/customer_surveys',
      dev_beta_only: false,
      hide_badage: false,
    },
    {
      id: 188,
      routerLink: '/home/51/reports/188/fvr_grid',
      dev_beta_only: false,
      hide_badge: false,
    },
    {
      id: 103,
      routerLink: '/home/51/reports/103/inventory_counts',
      dev_beta_only: false,
      hide_badage: false,
    },
    {
      id: 97,
      routerLink: '/home/51/reports/97/inventory_usage',
      dev_beta_only: false,
      hide_badge: false,
    },
    {
      id: 192,
      routerLink: '/home/51/reports/192/invoicing_time',
      dev_beta_only: false,
      hide_badge: false,
    },
    {
      id: 59,
      routerLink: '/home/51/reports/59/job_cost_report',
      dev_beta_only: false,
      hide_badge: false,
    },
    {
      id: 78,
      routerLink: '/home/51/reports/78/master_quote_grid',
      dev_beta_only: false,
      hide_badge: false,
    },
    {
      id: 260,
      routerLink: '/home/260/integerrors',
      hide_badge: true,
    },
    {
      id: 267,
      routerLink: '/home/267/customerrequestadmins',
      dev_beta_only: false,
      hide_badge: true,
    },
  ],
  DEBUG_HOMEPAGE: () => {
    // for andy
    if (HostContains(':4301')) {
    }
    // for matt
    if (HostContains(':4302')) {
    }
    // for luke
    if (HostContains(':4303') || HostContains('luke') || HostContains('alpha')) {
    }
    // for jordan
    if (HostContains(':4304')) {
    }
    return '/home/1/0';
  },
  DEV_HOMEPAGE: () => {
    // for andy
    if (HostContains(':4301')) {
    }
    if (HostContains(':4302')) {
    }
    // for luke
    if (HostContains(':4303') || HostContains('luke') || HostContains('alpha')) {
      return '/home/12/workorder/buckets/8';
    }
    // for jordan
    if (HostContains(':4304')) {
    }
    if (HostContains('devbeta')) {
      return '/home/12/workorder/buckets/8';
    }
    return '/home/11/vendors';
  },
  SKIPFVR: false,
  SQL_PATTERN: '[^\'|"]+',
  Only_Chrome: true,
  Regex: {
    // tslint:disable-next-line:max-line-length
    email: new RegExp(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/),
  },
  authentication: {
    authDataString: 'authData_V2',
    currentUserDataString: 'currentUserData_V2',
    signOutCheck: 'signOutCheck_V1',
    tokenBaseUrl: 'token',
    nesi1AuthUrl: '/api/authentication/token-login',
    nesi1SwitchUser: '/api/authentication/switchUser',
    nesi1AuthSignOutUrl: '/api/authentication/logout',
    refreshTokenMinutes: 20,
    cookieName: 'nesi2',
    showMenuBadge: false,
    currentURLString: 'currentURL_V4',
    currentURL_UserId: 'currentURL_userId_V4',
    ctrlURLString: 'ctrlURL_V1',
    originPath: 'originPath_V1',
    menuString: 'menus_V3',
    lastActiveTime: 'lastActiveTime_V1',
    lastPingTime: 'lastPingTime_V1',
    popupTime: 'popupTime_V1',
    releaseUser: 'releaseUser_V1',
    deniedUrl: 'deniedUrl_V1',
    nesi1IframeUrl: 'nesi1IframeUrl_V1',
    nesi1Iframe_UserId: 'nesi1Iframe_UserId_V1',
    nesi1IframeUrl_parent: 'nesi1IframeUrl_parent_V1',
  },
  apiURL: {
    host: () => {
      if (HostContains(':4300')) { return '//sparkops.api.localhost/' }
    /*  if (HostContains(':4300')) { return 'http://localhost:53450/' }*/
      if (HostContains('sparkops.web.localhost')) { return '//sparkops.api.localhost/' }

      if (HostContains(':4303')) { return '//localhost/' }
      if (HostContains(':4304')) { return '//api.localhost/' }
      if (HostContains(':4305')) { return '//localhost/' }
      if (HostContains(':4307')) { return '//password-api.nesi.ca/' }
      if (HostContains(':61164')) { return '//localhost/' }

      if (HostContains('netsuite')) { return 'https://netsuite-api.sparkopsdev.com/' }
      if (HostContains('qa')) { return 'https://qa-api.sparkopsdev.com/' }
      if (HostContains('uat')) { return 'https://uat-api.sparkopsdev.com/' }
      if (HostContains('ops.sparkpowercorp.com')) { return 'https://api.sparkpowercorp.com/' }

      return '//api.' + window.location.hostname + '/';
    },
    currentUser: {
      validateToken: 'api/CurrentUser/Active',
      signOutPath: 'api/CurrentUser',
      pageURL: 'api/CurrentUser/PageUrl/', // + pageId
      rebootTime: 'api/SignIn/RebootTime/', // + hostname
      ping: 'api/CurrentUser/ping',
      mobile_log: 'api/CurrentUser/MobileLog',
    },
    password: {
      validateResetToken: 'api/Password/ValidateResetToken',
      validate: 'api/Password/Validate',
      change: 'api/Password/Change',
      forgot: 'api/Password/Forgot',
      reset: 'api/Password/Reset',
      request: 'api/Password/Request'
    },
    layout: {
      menus: 'api/Layout/Menus',
      mobileMenus: 'api/Layout/MobileMenus',
      layoutProfiles: 'api/Layout/Profiles',
      fvrs: 'api/Layout/Fvrs',
      toDo: 'api/Layout/ToDo',
      tickets: 'api/Layout/Tickets',
      message: 'api/Layout/Message',
      quickExtensions: 'api/Layout/QuickExtension',
      whoDoIAsk: 'api/Layout/WhoDoIAsks',
      ucomingVacations: 'api/Layout/UpcomingVacations',
      averageDaysToInvoice: 'api/Layout/AverageDaysToInvoice',
      allActiveUsers: 'api/Layout/AllActive',
      switchUserList: 'api/Layout/SwitchUserList',
      switchUser: 'api/Layout/SwitchUser/', // + userid
      dashMessage: 'api/Layout/DashMessage/', // + businessunit Id
      defaultPage: 'api/Layout/DefaultPage/', // + page Id
      propertyValue: 'api/Layout/DefaultPage/PropertyValue/',
      changepassword: 'api/Layout/ChangePassword',
      syncLdap: 'api/Layout/SyncLdap',
      changepassword2: 'api/Layout/ChangePassword/2',
      changepasswordgetusername1: 'api/Layout/ChangePassword/getusername',
      idSearch: 'api/Layout/IdSearch',
    },
    core: {
      visibleBusinessUnitDropDownList: 'api/Core/VisibleBusinessUnit',
      activeBusinessUnitDropDownList: 'api/Core/ActiveBusinessUnit',
      paytypeHoursList: 'api/core/PayTypeHours/List/',
      phoneLog: 'api/core/PhoneLog/', // + userId/date
      profile: 'api/Core/Profile/',
      fileManager: {
        applicant: 'api/Core/FileManager/applicant/TreeNode/', // + applicantid
        employee: 'api/Core/FileManager/Employee/TreeNode/', // + memberid
        Quote: 'api/Core/FileManager/Quote/TreeNode/', // + woId
        workOrder: 'api/Core/FileManager/WorkOrder/TreeNode/', // + woId
        safetyFiles: 'api/Core/FileManager/Safety/TreeNode/', // + id =1
        customerFiles: 'api/Core/FileManager/Customer/TreeNode/$customer_id/$address_id', // + id =1
        vendorFiles: 'api/Core/FileManager/Vendor/TreeNode/$vendor_id', // + id =1
        customerAsset: 'api/Core/FileManager/CustomerAsset/TreeNode/',
        getFiles: 'api/Core/Filemanager/Files', // post url
        downloadFile: 'api/Core/Filemanager/Download', // post fileInfo
        uploadFiles: 'api/Core/Filemanager/File/Upload?savedUrl=', // saved path passed by querystring, post fileinfo[]
        deleteFiles: 'api/Core/Filemanager/File/Delete', // post body with fileinfo interface
        renameFile: 'api/Core/Filemanager/File/Rename', // post files[0] source file, files[1] dest file body with fileinfo interface
        copyFiles: 'api/Core/Filemanager/File/Copy', // post fileinfo[]
        moveFiles: 'api/Core/Filemanager/File/Move', // post fileinfo[]
        listDirectory: 'api/Core/Filemanager/Directory/List', // post dir body with directoryinfo interface
        createDirectory: 'api/Core/Filemanager/Directory/Create', // post dir body with directoryinfo interface
        deleteDirectory: 'api/Core/Filemanager/Directory/Delete', // post dir body with directoryinfo interface
        renameDirectory: 'api/Core/Filemanager/Directory/Rename', // post dirs[0], dirs[1] body with directoryinfo interface
      }
    },
    cache: {
      taxEntity: 'api/Cache/TaxEntity',
      businessUnitList: 'api/Cache/BusinessUnit',
    },
    page: {
      admintools: {
        selectmonth: 'api/Page/AdminTools/IntegError/Months',
        integerrors: 'api/Page/AdminTools/IntegError/Log/',
      },
      businessUnitList: 'api/Page/BusinessUnit',
      applicant: {
        base: 'api/Page/Applicant/',
        startNew_existing: 'api/Page/Applicant/StartNew/Existing',
        edit_profile: 'api/Page/Applicant/Edit/$applicant_id',
        updateGridField: 'api/Page/ApplicantHomeGrid/Update/', // +id,
      },
      customerrequestadmins: {
        base: 'api/Page/CustomerRequestAdmin/',
        create: 'api/Page/CustomerRequestAdminGrid/Create',
        update: 'api/Page/CustomerRequestAdminGrid/Update/$id',
        industrialTypes: 'api/Page/CustomerRequestAdminGrid/IndustrialTypes',
      },
      employee: {
        base: 'api/Page/Employee',
        advanceSearch: 'api/Page/EmployeesHomeGrid/AdvanceSearch',
        profile: 'api/Page/Employee/Profile',
        contact: 'api/Page/Employee/Contact/$member_id',
        termination: {
          start: 'api/Page/Employee/Termination/Start/$member_id',
          profile: 'api/Page/Employee/Termination/Profile/$member_id',
          checkList: 'api/Page/Employee/Termination/CheckList/$member_id/$type',
          checkListItem: 'api/Page/Employee/Termination/CheckList/Item/$member_id/$type',
        },
        review: {
          base: 'api/Page/Employee/Review',
          listProfile: 'api/Page/Employee/Review/List/$member_id',
          profile: 'api/Page/Employee/Review/Profile/$member_id/$review_id',
          scores: 'api/Page/Employee/Review/Scores/$member_id/$review_id',
          saveReview: 'api/Page/Employee/Review/SaveReview/$member_id/$review_id',
          saveMileStone: 'api/Page/Employee/Review/SaveMileStone/$member_id/$review_id',
          print_copy: 'api/Page/Employee/Review/PrintCopy/$member_id/$review_id',
        },
        privilege: {
          base: 'api/Page/Employee/Privilege/$member_id',
          switchUser: 'api/Page/Employee/Privilege/SwitchUser/$member_id',
          list: 'api/Page/Employee/Privilege/List/$member_id/$buid/$memberType',
          global: 'api/Page/Employee/Privilege/Global/$member_id',
          report: 'api/Page/Employee/Privilege/Report/$member_id',
        },
        offer: {
          notes: 'api/Page/Employee/Offer/Notes/$is_applicant/$member_id/$applicant_id/$offer_id',
          status: 'api/Page/Employee/Offer/Status/$is_applicant/$member_id/$applicant_id/$offer_id',
          detail: 'api/Page/Employee/Offer/Detail/$is_applicant/$member_id/$applicant_id/$offer_id',
          bm: 'api/Page/Employee/Offer/Detail/bm/$buid',
          extra: 'api/Page/Employee/Offer/Extras/$is_applicant/$member_id/$applicant_id/$offer_id',
          signBack: 'api/Page/Employee/Offer/SignBack/$is_applicant/$member_id/$applicant_id/$offer_id',
          wage: 'api/Page/Employee/Offer/Wage/$is_applicant/$member_id/$applicant_id/$offer_id',
          wageNotes: 'api/Page/Employee/Offer/WageNotes/$is_applicant/$member_id/$applicant_id/$offer_id', // post
          mileStone: 'api/Page/Employee/Offer/MileStone/$is_applicant/$member_id/$applicant_id/$offer_id', // get
          responsibilities: 'api/Page/Employee/Offer/Responsibilities/$is_applicant/$member_id/$applicant_id/$offer_id', // get
          filePath: 'api/Page/Employee/Offer/filePath',
        },
        employment: {
          List: 'api/Page/Employee/Employment/List/',
          applicantList: 'api/Page/Employee/Employment/List/Applicant/',
        },
        footPrints: {
          profile: 'api/Page/Employee/FootPrints/Profile/',
          options: 'api/Page/Employee/FootPrints/Options/',
          reassign: 'api/Page/Employee/FootPrints/Reassign',
        },
        files: {
          copyApplicant: 'api/Page/Employee/Files/CopyApplicant/',
        }, edit: {
          profile: 'api/Page/Employee/Edit/Profile/',
        },
        userInfo: {
          base: 'api/Page/Employee/UserInfo/Profile/$member_id',
          chargeout: 'api/Page/Employee/UserInfo/ChargeOut/',
          printBarcode: 'api/Page/Employee/UserInfo/PrintBarcode/$member_id', // data: copies
          reset_todo: 'api/Page/Employee/UserInfo/reset_todo/$member_id',
          duplicateSIN: 'api/Page/Employee/UserInfo/DuplicateSIN/$member_id',
          CheckReportsTo: 'api/Page/Employee/UserInfo/CheckReportsTo/$member_id',
        },
        it: {
          base: 'api/Page/Employee/it/Profile/$member_id',
          syncLdap: 'api/Page/Employee/it/SyncLdap/', // + member id
          resetPassword: 'api/Page/Employee/it/ResetPassword/$member_id', // + member id
        },
        daysOff: {
          Override: 'api/Page/Employee/DaysOff/Override/$member_id',
        },
        disciplinary: {
          get: 'api/Page/EmployeeDiscplinaryGrid/Get/@id', // + id
          save: 'api/Page/EmployeeDiscplinaryGrid/Save',
          delete: 'api/Page/EmployeeDiscplinaryGrid/Delete',
          filePath: 'api/Page/EmployeeDiscplinaryGrid/filePath',
        },
        wage: {
          base: 'api/Page/Employee/Wage/Vacation/$member_id',
          wage: 'api/Page/Employee/Wage/Wage/$member_id',
        },
      },
      vendors: {
        base: 'api/Page/Vendors',
        profile: 'api/Page/Vendors/Profile', // get
        startNew: {
          base: 'api/Page/Vendors/StartNew', // get, post
          checkName: 'api/Page/Vendors/StartNew/CheckName', // Post + data: string
          checkPhone: 'api/Page/Vendors/StartNew/CheckPhone', // Post + data: string
        },
        edit: 'api/Page/Vendors/Edit/$vendor_id', // post = vendor_id
        bvcontact: 'api/Page/Vendors/BVContact/$vendor_id/$bv_index', // post = vendor_id
        address: 'api/Page/Vendors/Address/$vendor_id', // post = vendor_id
        phonenumber: 'api/Page/Vendors/PhoneNumber/$vendor_id', // post = vendor_id
        specialties: 'api/Page/Vendors/Specialties/$vendor_id', // post = vendor_id
        market: 'api/Page/Vendors/Market/$vendor_id', // post = vendor_id
        accounting: 'api/Page/Vendors/Accounting/$vendor_id', // post = vendor_id
        notes: 'api/Page/Vendors/Notes/$vendor_id', // post = vendor_id
      },
      customers: {
        base: 'api/Page/Customers',
        profile: 'api/Page/Customers/Profile', // get
        search: 'api/Page/Customers/Search', // post model
        postcodeSearch: 'api/Page/Customers/PostCodeSearch', // post to get all postcode
        new: {
          profile: 'api/Page/Customers/New/Profile', // get
          save: 'api/Page/Customers/New/Save', // post
          businessUnitPorifle: 'api/Page/Customers/New/BusinessUnitPorifle/', // get + buId
          startNew: 'api/Page/Customers/New/StartNew', // Post
          checkName: 'api/Page/Customers/New/CheckName', // Post + data: string
          checkPhone: 'api/Page/Customers/New/CheckPhone', // Post + data: string
        },
        edit: {
          profile: 'api/Page/Customers/Edit/Profile/$customer_id', // get + customer_id
          save: 'api/Page/Customers/Edit/Save/$customer_id', // post = customer_id
          changeQC: 'api/Page/Customers/Edit/changeQC/$customer_id', // post = customer_id
          address: 'api/Page/Customers/Edit/Address/$customer_id', // get/post/delete/patch  customer_id/$address_id
          customerbusinessunit: 'api/Page/Customers/Edit/CustomerBusinessUnit/$customer_id', // get/post/delete/patch  customer_id/$address_id
          specialties: 'api/Page/Customers/Edit/Specialties/$customer_id', // get/post/delete/patch  customer_id/
          phoneNumber: 'api/Page/Customers/Edit/PhoneNumber/$customer_id/$address_id', // get/post/delete/patch  customer_id/
          contact: 'api/Page/Customers/Edit/Contact/$customer_id/$address_id', // get/post/delete/patch  customer_id/$address_id
          notes: 'api/Page/Customers/Notes/$customer_id/$address_id', // get/post/delete/patch  customer_id/$address_id
          notesProfile: 'api/Page/Customers/Notes/Profile/$customer_id/$address_id', // get/post/delete/patch  customer_id/$address_id
          notesGetArEmails: 'api/Page/Customers/Notes/ArEmails/$customer_id/$address_id', // get/post/delete/patch  customer_id/$address_id
          // tslint:disable-next-line:max-line-length
          notesGetInvoiceNotes: 'api/Page/Customers/Edit/Notes/InvoiceNotes/$customer_id/$address_id', // get/post/delete/patch  customer_id/$address_id
          contactRandomPassword: 'api/Page/Customers/Edit/Contact/RandomPassword', // get
          contactIsDuplicate: 'api/Page/Customers/Edit/Contact/IsDuplicate/$customer_id/$address_id', // post  customer_id/$address_id
          addressAccounting: 'api/Page/Customers/Edit/AddressAccounting/$customer_id', // get/post/delete/patch  customer_id/$address_id
          accountingSetting: 'api/Page/Customers/Edit/AccountingSettings/Setting/$customer_id', // get/post/delete/patch  customer_id
          accountingArNotes: 'api/Page/Customers/Edit/AccountingSettings/ARNotes/$customer_id', // get/post/delete/patch  customer_id
          accountingRates: 'api/Page/Customers/Edit/AccountingSettings/Rates/$customer_id/$address_id', // get  customer_id/$bu_id
          // tslint:disable-next-line:max-line-length
          accountingRatesDelete: 'api/Page/Customers/Edit/AccountingSettings/Rates/Delete/$customer_id/$address_id', // get  customer_id/$bu_id
          accountingRatesOverrideSetting: 'api/Page/Customers/Edit/AccountingSettings/Rates/OverrideSetting/',
        },
        sales: {
          base: 'api/Page/Customers/Sales/$customer_id/$address_id', // get + customer_id/address_id
          history: 'api/Page/Customers/Sales/History/$customer_id/$address_id', // get + customer_id/address_id
          profile: 'api/Page/Customers/Sales/Profile/$customer_id/$address_id', // get + customer_id/address_id
        }
      },
      workOrder: {
        base: 'api/Page/WorkOrder',
        profile: 'api/Page/WorkOrder/Profile',
        scanned_file_updloaded: 'api/Page/WorkOrder/ScannedFile/Uploaded',
        scanned_file_path: 'api/Page/WorkOrder/ScannedFile/FilePath',
        renname_file: 'api/Page/WorkOrder/RenameFile/',
        scanned_file: 'api/Page/WorkOrder/ScannedFile/',
        buckets: 'api/Page/WorkOrder/Buckets/',
        saveLayout: 'api/Page/WorkOrder/SaveLayout',
        businessUnit_summary: 'api/Page/WorkOrder/BusinessUnitSummary/', // + buId
        summaryDetail: 'api/Page/WorkOrder/SummaryDetail/', // + buId
        edit: {
          main: 'api/Page/WorkOrder/Edit/Main/$bu_id/$str_wo_id',
          main_tabs: 'api/Page/WorkOrder/Edit/MainTabs/$bu_id/$str_wo_id',
          wocomments: 'api/Page/WorkOrder/Edit/WoComments/$bu_id/$str_wo_id',
        },
      },
      purchaseorder: {
        base: 'api/Page/PurchaseOrder',
        apStatus: 'api/Page/PurchaseOrder/APStatus',
        updateAPNotes: 'api/Page/PurchaseOrder/UpdateAPNotes',
        updateStatus: 'api/Page/PurchaseOrder/UpdateStatus',
        profile: 'api/Page/PurchaseOrder/Profile',
        saveLayout: 'api/Page/PurchaseOrder/SaveLayout',
        businessUnit_summary: 'api/Page/PurchaseOrder/BusinessUnitSummary/', // + buId
        summaryDetail: 'api/Page/PurchaseOrder/SummaryDetail/', // + buId
        creditCard: 'api/Page/PurchaseOrder/CreditCard',
        creditCardSave: 'api/Page/PurchaseOrder/CreditCardSave'
      },
      messages: {
        base: 'api/Page/Messages',
        read: 'api/Page/Messages/Read/',
        filePath: 'api/Page/Messages/filePath',
        userList: 'api/Page/Messages/UserList',
        inbox: 'api/Page/Messages/Inbox',
        sent: 'api/Page/Messages/Sent',
        deleted: 'api/Page/Messages/Deleted',
        deleted_inbox: 'api/Page/Messages/Deleted/Inbox',
        deleted_sent: 'api/Page/Messages/Deleted/Sent',
        delete: 'api/Page/Messages/Delete/', // post ids[] + type 1--inbox, 2 sent
      },
      homepage: {
        base: 'api/Page/HomePage/',
        profile: 'api/Page/HomePage/Profile',
        contact: 'api/Page/HomePage/Contact',
        todo: 'api/Page/HomePage/Todo',
        autoBingo: 'api/Page/HomePage/AutoBingo',
        invoiceService: 'api/Page/HomePage/InvoiceService',
        slowPage: 'api/Page/HomePage/SlowPage',
      },
      demo: {
        checkpassword: 'api/Page/Demo/checkpassword', // post data
        getusername: 'api/Page/Demo/getUserName', // get
      },
      shared: {
        customerContactTitles: 'api/Shared/Customer/Contact/Titles', // get customer contact titles.
        customerContactNew: 'api/Shared/Customer/Contact/New/', // + custId post a new contact of the customer.
        CustomerContactEdit:'api/Shared/Customer/Contact/Edit/',
        pickList: {
          settings_matchWholeWord: 'api/Page/Shared/PickList/Settings/MatchWholeWord',
          InventoryPicture: 'api/Page/Shared/PickList/InventoryPicture/', // + masterid
          InventorySearchHistory: 'api/Page/Shared/PickList/InventorySearchHistory', // +quote_id/revision
          QuoteSearchInventory: 'api/Page/Shared/PickList/Quote/SearchInventory/', // +quote_id/revision + post model query
          recentWos: 'api/Page/Shared/PickList/Quote/RecentWos/', // + quote_id/revision
          recentQuotes: 'api/Page/Shared/PickList/Quote/RecentQuotes/', // + quote_id/revision
          quote: 'api/Page/Shared/PickList/Quote/', // + quote_id/revision
          quoteSections: 'api/Page/Shared/PickList/Quote/Sections/', // + quote_id/revision
          specificSections: 'api/Page/Shared/PickList/specificSections/', // + buid
          quoteSectionList: 'api/Page/Shared/PickList/Quote/SectionList/', // + quote_id/revision
          quoteNote: 'api/Page/Shared/PickList/Quote/Note', // post id, note
          quoteLineUpdate: 'api/Page/Shared/PickList/Quote/Line/', // patch model
          quoteInventory: 'api/Page/Shared/PickList/Quote/Inventory/', // + quote_id/revision/masterid
          quoteInventories: 'api/Page/Shared/PickList/Quote/Inventories/', // + quote_id/revision/masterid
          reOrderQuoteWorkSheet: 'api/Page/Shared/PickList/Quote/ReOrder/', // post quote_id/revision {id, index}
          addQuoteLabor: 'api/Page/Shared/PickList/Quote/Labor/', // post quote_id/revision models[]
          addQuoteMaterial: 'api/Page/Shared/PickList/Quote/Material/', // post quote_id/revision models[]
          addQuoteMaterials: 'api/Page/Shared/PickList/Quote/Materials/', // post quote_id/revision models[]
          memberTypes: 'api/Page/Shared/PickList/MemberTypes/', // + bu id
          kitteds: 'api/Page/Shared/PickList/Kitteds', // + kitted id
          groups: 'api/Page/Shared/PickList/Groups', // + group id
          customers: 'api/Page/Shared/PickList/Customers/', // + group id
          customersQuotes: 'api/Page/Shared/PickList/CustomersQuotes/', // + buid/custid
          queryQuotes: 'api/Page/Shared/PickList/QueryQuotes', // post query string
          customersWos: 'api/Page/Shared/PickList/CustomersWos/', // + buid/custid
          queryWos: 'api/Page/Shared/PickList/QueryWos', // post query string
          quoteParts: 'api/Page/Shared/PickList/QuoteParts/', // + quoteId
          woParts: 'api/Page/Shared/PickList/WoParts/', // + woId
          quoteQueryPart: 'api/Page/Shared/PickList/Quote/QueryPart/', // + buid, post data
          quotePart: 'api/Page/Shared/PickList/Quote/Part/', // + buid/partno
          editSections: 'api/Page/Shared/PickList/Quote/EditSections/', // + quoteid/revision get/post/put/delete
          costHandler: 'api/Page/Shared/PickList/Quote/CostHandler/', // + quoteid/revision/qty/cost
          copyMoveLines: 'api/Page/Shared/PickList/Quote/CopyMoveLines', // post
          applyDiccount: 'api/Page/Shared/PickList/Quote/ApplyDiscount/', // post + quoteid/revision post
          LineIsChecked: 'api/Page/Shared/PickList/Quote/LineIsChecked/', //  + quoteid/revision post
          chargeOut: 'api/Page/Shared/PickList/ChargeOut/', // + buid/mtid
        },
      },
      releaseSystem: {
        rebootTime: 'api/ReleaseSystem/RebootTime', // + /hostname or post to set value
      },
      quotes: {
        profile: 'api/Page/Quotes/Profile', // get quote profile, no argument.
        getActiveRevision: 'api/Page/Quotes/ActiveRevision/', // get quote acitve revision + quoteId
        profileFilter: 'api/Page/Quotes/ProfileFilter', // get quote profile, no argument.
        search: 'api/Page/Quotes/Search', // post to get search result
        exist: 'api/Page/Quotes/Exist/', // check the specific quote existing by its id
        searchInit: 'api/Page/Quotes/Search/Init', // get search initilize
        searchUserList: 'api/Page/Quotes/Search/UserList/', // + buId get search userList
        new: 'api/Page/Quotes/New', // post add a new quote, no argument.
        newProfile: 'api/Page/Quotes/New/Profile', // get quote new profile, no argument.
        newUserList: 'api/Page/Quotes/New/UserList/', // + buId get search userList
        opportunityFilter: 'api/Page/Quotes/Edit/Opportunity/', // + customer_id to get opportuinity
        newCustomerFilter: 'api/Page/Quotes/New/Customer/Filter/', // + filter string get search customer
        newCustomerProfile: 'api/Page/Quotes/New/Customer/Profile/', // + customer id
        newCustomerAddress: 'api/Page/Quotes/New/Customer/Address/', // + customer id
        newCustomerBDM: 'api/Page/Quotes/New/Customer/BDM/', // + customer id + business unit id
        newCustomerAddressContact: 'api/Page/Quotes/New/Customer/Address/Contact/', // + customer id
        newCustomerQuotes: 'api/Page/Quotes/New/Customer/Quotes/', // + customer id
        editInit: 'api/Page/Quotes/Edit/Init/',  // + quoteId/revision
        setActiveRevision: 'api/Page/Quotes/Edit/ActiveRevision/',  // + quoteId/revision
        duplicate: 'api/Page/Quotes/Edit/Duplicate/',  // + quoteId/revision
        updateLastPrintDate: 'api/Page/Quotes/Edit/UpdateLastPrintDate/',  // + quoteId/revision
        editUpdateAll: 'api/Page/Quotes/Edit/UpdateAll',  // + view model data.
        editUpdate: 'api/Page/Quotes/Edit/Update',  // + view model data.
        doneFollowUp: 'api/Page/Quotes/Edit/DoneFollowUp/',  // post null data + followup id.
        NewReveneLine:'api/Page/Quotes/Edit/NewReveneLine/',
        editAddress: 'api/Page/Quotes/Edit/Address/',  // + customerId/AddressId, addressId==0 get the first 1
        editContact: 'api/Page/Quotes/Edit/Contact/',  // + contactId
        editNewId: 'api/Page/Quotes/Edit/Detail/NewId/',  // + quoteId/revisionId/typeid
        editNewSection: 'api/Page/Quotes/Edit/Detail/NewSection/',  // + quoteId/revisionId/lineid
        saveNewIds: 'api/Page/Quotes/Edit/Detail/NewIds/',  // post + quoteId/revisionId/typeid
        detailDiv: 'api/Page/Quotes/Edit/Detail/Div/',  // + quoteId/revisionId/typeid
        deleteDetail: 'api/Page/Quotes/Edit/Detail/Delete/',  // + id/del_assoc
        deleteNote: 'api/Page/Quotes/Edit/Note/',  // + id
        getFollowupHistory: 'api/Page/Quotes/Edit/FollowUpHistory/',  // + quoteId/revisionId
        secheduleFollowup: 'api/Page/Quotes/Edit/ScheduleFollowUp',  // post data
        updateStatus: 'api/Page/Quotes/Edit/UpdateStatus/',  //  post data
        reOrder: 'api/Page/Quotes/Edit/ReOrder/',  //  post data
        initStrategy: 'api/Page/Quotes/Strategy/Init/',  // + quote_id
        strategyStage1: 'api/Page/Quotes/Strategy/Stage1/',  // + quote_id
        strategyKillStage1: 'api/Page/Quotes/Strategy/KillStage1/',  // + quote_id post string data
        strategyKillQuote: 'api/Page/Quotes/Strategy/KillQuote/',  // + quote_id post string data
        strategyUpdateItem: 'api/Page/Quotes/Strategy/UpdateScheduleItem/', // + quote_id post model
        processQuestionHistory: 'api/Page/Quotes/Strategy/ProcessQuestionHistory/', // + quote_id + type
        processQuestionHistoryUpdate: 'api/Page/Quotes/Strategy/ProcessQuestionHistoryUpdate/', // + quote_id
      },
      ticketFlyOut: {
        group: 'api/Page/TicketFlyOut/Group', // no argument to get grouplist + '/{groupId}' to get types and perts.
        relatedTickets: 'api/Page/TicketFlyOut/RelatedTickets/', // +{pageid}
        save: 'api/Page/TicketFlyOut/Save', // post to save
        file: 'api/Page/TicketFlyOut/File?name=', // upload/get/delete file name is temp file name
      },
      timesheet: {
        delete: 'api/Page/Timesheet/delete/', // + userId/id
        list: 'api/Page/Timesheet/List/', // + userId/date
        editPermission: 'api/Page/Timesheet/EditPermission/', // + userId/date
        visibleBusinessUnitDropDownList: 'api/Page/TimeSheet/VisibleBusinessUnit',
        userListByBusinessUnitID: 'api/Page/TimeSheet/User/', // + Buid
        profile: 'api/Page/Timesheet/profile',
        pastDays: 'api/Page/Timesheet/PastDays/', // + userId
        Country:'api/Page/Timesheet/Country/', // + BuId
        startDate: 'api/Page/Timesheet/StartDate/', // + userId
        cutPO:'api/Page/Timesheet/WorkOrder/CutPO/', // +WoId
        provinceList: 'api/Page/Timesheet/ProvinceList/', // + userId
        defaultProvince: 'api/Page/Timesheet/DefaultProvince/', // + buid
        payperiodTotalHours: 'api/Page/Timesheet/PayperiodTotalHours/', // + userId
        ValueFromScheduler: 'api/Page/Timesheet/ValueFromScheduler/', // + userId/date
        WorkOrder: 'api/Page/Timesheet/WorkOrder', // patch to update data,  post to insert data
        Quote: 'api/Page/Timesheet/Quote', // patch to update data,  post to insert data
        Telem: 'api/Page/Timesheet/Telem', // patch to update data,  post to insert data
        Shop: 'api/Page/Timesheet/Shop', // patch to update data,  post to insert data
        ShopShopType: 'api/Page/Timesheet/Shop/ShopType',
        UpdateProject : 'api/Page/Timesheet/Projects/Update',
        InsertProject : 'api/Page/Timesheet/Projects/Insert',//
        Projects: 'api/Page/Timesheet/Projects/ProjectsList',// //get project list
        QuoteCustomer: 'api/Page/Timesheet/Quote/Customer/', // + buId
        QuoteCustomerName: 'api/Page/Timesheet/Quote/Customer/Name/', // + custId
        QuoteQuote: 'api/Page/Timesheet/Quote/Quote/', // + buId/custId
        QuoteQuoteName: 'api/Page/Timesheet/Quote/Quote/Name/', // + quotId
        Signature:'api/Page/Timesheet/WorkOrder/Signature/',//
        PreviewPdf:'api/Page/Timesheet/WorkOrder/PreviewPdf/',//
        SendingEmail:'api/Page/Timesheet/WorkOrder/Email',
        WorkOrderLabour: 'api/Page/Timesheet/WorkOrder/Labour/', // + {buid}/{userId}/{payTypeId}
        WorkOrderUnLinkTimeSheet: 'api/Page/Timesheet/WorkOrder/UnLinkTimeSheet/', // + buId
        WorkOrderCustomer: 'api/Page/Timesheet/WorkOrder/Customer/', // + buId
        WorkOrderWos: 'api/Page/Timesheet/WorkOrder/WOs/', // + buId/userId/customerId or + woId to get a wo result
        WorkOrderComments: 'api/Page/Timesheet/WorkOrder/Comment/', // + buId/userId/woId
        WorkOrderCommentsDisabled: 'api/Page/Timesheet/WorkOrder/CommentDisabled/', // + buId/userId/woId
        IsPrevailingWage: 'api/Page/Timesheet/WorkOrder/IsPrevailingWage', //userId/timesheetId
        IsPrevailingWageEnabledForWo: 'api/Page/Timesheet/WorkOrder/IsPrevailingWageEnabledForWo', //userId/woId
        newCustomerAddressContact: 'api/Page/Timesheet/WorkOrder/Customer/Address/Contact/', // + customer id
        GetCustomerAddressID:'/api/Page/Timesheet/WorkOrder/Address/Contact/',
        TsLitePaytypes: 'api/Page/TimeSheet/Workorder/TsLitePaytypes/',
        expense: {
          save: 'api/Page/Timesheet/Expense/Save', // post form
          delete: 'api/Page/Timesheet/Expense/Delete/', // delete + id_expense
          profile: 'api/Page/Timesheet/Expense/Profile', // null value is for current user , /{userId} is for selected user
          seller: 'api/Page/Timesheet/Expense/Seller', // post search string {data: ''}
          receiptRequired: 'api/Page/Timesheet/Expense/ReceiptRequired/', // + selected UserId/categoryId
          currency:'api/Page/Timesheet/Expense/Currency', //+ selected Wo Number
          history: 'api/Page/Timesheet/Expense/History', // get current user's history no argument
          perDiemSave: 'api/Page/Timesheet/Expense/PerDiem/Save', // post form + id
          perDiemProfile: 'api/Page/Timesheet/Expense/PerDiem/Profile',
          perDiemBuProfile: 'api/Page/Timesheet/Expense/PerDiem/Profile/',  // + business Unit id
        },
        vacation: {
          paytypeId: 'api/Page/Timesheet/Vacation/paytypeId', // get
          delete: 'api/Page/Timesheet/Vacation/Delete/', // delete + id
          save: 'api/Page/Timesheet/Vacation/Save', // post
          review: 'api/Page/Timesheet/Vacation/Review', // post
          history: 'api/Page/Timesheet/Vacation/History', // get current user's history no argument
          notes: 'api/Page/Timesheet/Vacation/Notes/', // + vacation_id
          avaliableAtDate: 'api/Page/Timesheet/Vacation/Available/AtDate/', // + date
        },
        pel: {
          profile: 'api/Page/Timesheet/PEL', // delete + id
          save: 'api/Page/Timesheet/PEL/Save', // post
          delete: 'api/Page/Timesheet/PEL/Delete/', // post
          review: 'api/Page/Timesheet/PEL/Review', // post
        },
        bankpay: {
          summary: 'api/Page/Timesheet/Bankpay/Summary', //  no argument
          ledger: 'api/Page/Timesheet/Bankpay/Ledger', // empty or add /{payperiodId}
          bankHours: 'api/Page/Timesheet/Bankpay/BankHours', // post data: hours
          retractHours: 'api/Page/Timesheet/Bankpay/Retract/', // delete  id
          withdrawHours: 'api/Page/Timesheet/Bankpay/WithdrawHours', // post data: hours
        },
      },

      reports: {
        masterWorkOrder: {
          activeRam: 'api/Page/MasterWorkOrder/Ram',
          updateActiveRam: 'api/Page/MasterWorkOrder/UpdateRam',
          edit: 'api/Page/MasterWorkOrder/Edit'
        },
        masterWorkOrderV2: {
          activeRam: 'api/Page/MasterWorkOrderV2/Ram',
          updateActiveRam: 'api/Page/MasterWorkOrderV2/UpdateRam',
          edit: 'api/Page/MasterWorkOrderV2/Edit'
        },
        masterQuoteGrid: {
          edit: 'api/Page/MasterQuoteGrid/Edit',
          chance: 'api/Page/MasterQuoteGrid/Chance',
          addNote: 'api/Page/MasterQuoteGrid/Note',
          updateChance: 'api/Page/MasterQuoteGrid/updateChance'
        },
        masterPurchasesGrid: {
          edit: 'api/Page/MasterPurchasesGrid/Edit',
          apStatus: 'api/Page/MasterPurchasesGrid/apStatus',
          updateApStatus: 'api/Page/MasterPurchasesGrid/updateApStatus'
        },
        masterOutstandingInvoicesGrid: {
          confirmPaidDate: 'api/Page/MasterOutstandingInvoicesGrid/confirmPaidDate',
          invoiceNotes: 'api/Page/MasterOutstandingInvoicesGrid/invoiceNotes/',
          addOrUpdateNote: 'api/Page/MasterOutstandingInvoicesGrid/addOrUpdateNote',
          edit: 'api/Page/MasterOutstandingInvoicesGrid/Edit',
          customerStatus: 'api/Page/MasterOutstandingInvoicesGrid/customerStatus',
          invoiceStatus: 'api/Page/MasterOutstandingInvoicesGrid/invoiceStatus'
        },
      }
    },
    logging: {
      error: 'api/Error'
    }
  },
  opens: {
    quoteEdit: '/#/opens/65/quotes/@id/@rev',
  },
  Nesi1URL: {
    host: () => {
      if (HostContains(':4300')) {
       return 'http://sparkops.web.localhost';
       //return 'http://localhost:65121';
      }

      if (HostContains(':4307')) {
        return 'http://password.nesi.ca';
      }
      if (HostContains(':4304')) { return '//localhost' }
      if (HostContains(':4305')) { return '//localhost:61164' }
      if (HostContains(':611')) {
        return '//' + window.location.hostname + ':' + window.location.port;
      } if (HostContains(':4448')) {
        return '//' + window.location.hostname + ':' + window.location.port;
      }
      return '//' + window.location.hostname;
    },
    Fvr: '/sections/hr/fvr/index.aspx?is_n1=true&id=',
    inventory: '/sections/member/inventory/index.aspx?a=get&tab=G&id=',
    TicketsList: '/sections/member/tickets/list_view.aspx?page_id=170&is_n1=true',
    TicketsMobile: '/home/1/170',
    Tickets: '/sections/member/tickets/index.aspx?page_id=170&is_n1=true',
    getFile: '/_tools/get_file/index.aspx?file_id=@id',
    // signIn: '/login.aspx',
    // signOut: '/logout.aspx',
    QuickExtension: '/sections/member/Extension_Directory/index.aspx?is_n1=true',
    ExpenseReimbursement: '/sections/member/expense/index.aspx?is_n1=true',
    VacationRequests: '/sections/member/vacation/index.aspx?is_n1=true',
    FileManager: '/filemanager.aspx?is_n1=true',
    WikiHelp: '/sections/messaging/popup_wiki_help.aspx?is_n1=true',
    member_access: '/sections/hr/member_access/index.aspx?page_id=96&type=member',
    // tslint:disable-next-line:max-line-length
    printQuoteWorksheet: '/sections/reports/print_quote_worksheet/index.aspx?origin=quote&quoteid=@quote_id&revision=@revision&stuff=85&Q=@Q&T=@T',
    printQuote: '/sections/reports/print_quote/index.aspx?quoteid=@quoteid&from_process=1&type=w',
    printOffer: '/sections/hr/member/offer_print_off.aspx?moid=@moid',
    upload_offer: '/sections/hr/member/member_offer.aspx?save_file_moid=@moid',
    printReview: '/sections/hr/member/print_review.aspx?rid=@reviewid&locked=@locked&is_worksheet=@t',
    quoteWorkorder: '/sections/workorder/index.aspx?woprog_id=0&business_unit_id=@buid&fromquoteid=@quoteid',
    openticket: '/sections/member/tickets/ticketpage.aspx?issue=@ticketid',
    createticket: '/sections/member/tickets/CreateIssue.aspx?from=milestone&ms_id=@id',
    customer: {
      editCustomer: '/#/opens/10/customers/',
    },
    contact: {
      contactDisplay: '/sections/hr/member/membercontactdisplay.aspx?is_n1=true&memid=',
    },
    requestCustOrVendor:'/modules/request.aspx?type=@type&id=@customer_id',
  },
  ResponsiveConfig: {
    breakPoints: {
      xs: { max: 640 },
      sm: { min: 641, max: 959 },
      md: { min: 960, max: 1279 },
      lg: { min: 1280, max: 1919 },
      xl: { min: 1920 }
    },
    debounceTime: 100 // allow to debounce checking timer
  },
  SignalR: {
    host: () => {
      if (HostContains(':4300')) { return '//sparkops.hub.localhost/' }
      /*if (HostContains('http://localhost:65121')) { return 'http://localhost:61219/' }*/
      if (HostContains(':4304')) { return '//hub.localhost/' }
      if (HostContains('nesi.web.localhost')) { return '//nesi.hub.localhost/' }
      if (HostContains('sparkops.web.localhost')) { return '//sparkops.hub.localhost/' }
      if (HostContains('localhost')) { return '//hub.localhost/' }

      if (HostContains('uat')) { return 'https://uat-hub.sparkopsdev.com/' }
      if (HostContains('qa')) { return 'https://qa-hub.sparkopsdev.com/' }

      if (HostContains('ops.sparkpowercorp.com')) { return 'https://hub.sparkpowercorp.com/' }

      return 'https://qa-hub.sparkopsdev.com/';
    },
    qs: 'Spark Ops',
    hubName: 'EventsHub',
    fireToOtherEvent: 'SendToOthers',
    fireToAllEvent: 'SendToAll',
    Events: {
      self_KeepSiginAllWindow: {
        name: 'self_keep_sigin_in_all_window',
        message: ''
      },
      self_signOutAllWindow: {
        name: 'self_sign_out_all_window',
        message: ''
      },
      self_signInAllWindow: {
        name: 'self_sign_in_all_window',
        message: ''
      },
      SignIn: {
        name: 'signalr_signin',
        message: 'signed in'
      },
      SignOut: {
        name: 'signalr_signout',
        message: 'signed out'
      },
      chatDashMessage: {
        name: 'signalr_chatDashMessage',
        message: ' has a new chat message'
      },
      reboot: {
        name: 'signalr_reboot',
        message: 'reboot'
      },
      newMessage: {
        name: 'new_message',
        message: 'You have a new message from @name.'
      },
      Test: {
        name: 'signalr_test',
        message: 'test'
      }
    }
  },
  messageLifeTime: 5000,
  timer: {
    checkActiveTime: 5 * 60,
    countDownTimeWhenAutoSignOut: 10 * 60,
  },
  LOG: (msg: any = 'unknow', obj: any = 'unknow') => {
    if (HostContains('localhost') || HostContains('stage') || HostContains('alpha') || HostContains('qa') || HostContains('luke')) {
      if (String(msg).toLowerCase().startsWith('[object')) {
        msg = JSON.stringify(msg);
        console.dir(msg);
      }
      if (prevLog.msg !== String(msg) || prevLog.obj !== obj) {
        prevLog.msg = String(msg);
        prevLog.obj = obj;
        console.log('[Source]:' + obj + ' [Value]:' + String(msg));
      }
    }
  },
  ApiUrls: {
    GET_LAYOUTS: 'api/Layout/GetLayouts',
    ADD_LAYOUT: 'api/Layout/AddLayout',
    UPDATE_LAYOUT: 'api/Layout/UpdateLayout',
    DELETE_LAYOUT: 'api/Layout/deleteLayout/',
  },
  Constants: {
    LAYOUT_DELETE: 1,
    LAYOUT_NEW: 2,
    RECORD_DELETE: 3
  }
}

export function ResponsiveDefinition() {
  return new ResponsiveConfig(CONFIG.ResponsiveConfig);
};


export let LOGGING_ERROR_HANDLER_OPTIONS: LoggingErrorHandlerOptions = {
  rethrowError: false,
  unwrapError: false,
};

export let ERROR_OUTPUT_OPTIONS: ErrorOutputOptions = {
  sendToConsole: true,
  sendToGrowlMessage: true,
  sendToServer: true
}


export function downloadcsv(data: any, exportFileName: string) {
  const csvData = convertToCSV(data);

  const blob = new Blob([csvData], { type: 'text/csv;charset=utf-8;' });

  if (navigator.msSaveBlob) { // IE 10+
    navigator.msSaveBlob(blob, createFileName(exportFileName))
  } else {
    const link = document.createElement('a');
    if (link.download !== undefined) { // feature detection
      // Browsers that support HTML5 download attribute
      const url = URL.createObjectURL(blob);
      link.setAttribute('href', url);
      link.setAttribute('download', createFileName(exportFileName));
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    }
  }
}

function convertToCSV(objarray: any) {
  const array = typeof objarray !== 'object' ? JSON.parse(objarray) : objarray;

  let str = '';
  let row = '';

  // tslint:disable-next-line:forin
  for (const index in objarray[0]) {
    // Now convert each value to string and comma-separated
    row += index + ',';
  }
  row = row.slice(0, -1);
  // append Label row with line break
  str += row + '\r\n';

  for (let i = 0; i < array.length; i++) {
    let line = '';
    // tslint:disable-next-line:forin
    for (const index in array[i]) {
      if (line !== '') { line += ',' }
      line += JSON.stringify(array[i][index]);
    }
    str += line + '\r\n';
  }
  return str;
}

function createFileName(exportFileName: string): string {
  const date = new Date();
  return (exportFileName +
    date.toLocaleDateString() + '_' +
    date.toLocaleTimeString()
    + '.csv')
}

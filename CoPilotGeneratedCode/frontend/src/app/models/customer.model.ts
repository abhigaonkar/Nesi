export interface Customer {
  id: number;
  customerNumber: string;
  name: string;
  businessUnitId?: number;
  businessUnitName?: string;
  contactName?: string;
  email?: string;
  phone?: string;
  address?: string;
  creditLimit?: number;
  paymentTermsDays?: number;
  accountManagerId?: number;
  accountManagerName?: string;
  notes?: string;
  isActive: boolean;
  createdAt: Date;
  updatedAt?: Date;
  
  // Related data when includeDetails is true
  addresses?: CustomerAddress[];
  contacts?: CustomerContact[];
  customerNotes?: CustomerNote[];
}

export interface CustomerAddress {
  id: number;
  customerId: number;
  addressType: AddressType;
  addressLine1: string;
  addressLine2?: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
  isDefault: boolean;
}

export interface CustomerContact {
  id: number;
  customerId: number;
  name: string;
  title?: string;
  email?: string;
  phone?: string;
  cellPhone?: string;
  isPrimary: boolean;
}

export interface CustomerNote {
  id: number;
  customerId: number;
  note: string;
  createdByUserId: number;
  createdByUserName?: string;
  createdAt: Date;
}

export enum AddressType {
  Billing = 0,
  Shipping = 1,
  Both = 2
}

export interface CreateCustomerRequest {
  name: string;
  businessUnitId?: number;
  contactName?: string;
  email?: string;
  phone?: string;
  address?: string;
  creditLimit?: number;
  paymentTermsDays?: number;
  accountManagerId?: number;
  notes?: string;
}

export interface UpdateCustomerRequest {
  id: number;
  name: string;
  businessUnitId?: number;
  contactName?: string;
  email?: string;
  phone?: string;
  address?: string;
  creditLimit?: number;
  paymentTermsDays?: number;
  accountManagerId?: number;
  notes?: string;
}

export interface AddCustomerAddressRequest {
  customerId: number;
  addressType: AddressType;
  addressLine1: string;
  addressLine2?: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
  isDefault: boolean;
}

export interface AddCustomerContactRequest {
  customerId: number;
  name: string;
  title?: string;
  email?: string;
  phone?: string;
  cellPhone?: string;
  isPrimary: boolean;
}

export interface AddCustomerNoteRequest {
  customerId: number;
  note: string;
  createdByUserId: number;
}

export interface GetCustomersResult {
  customers: Customer[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

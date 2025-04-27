export class MasterCompany {
    valueId?:number;
    companyName?:string;
    pocName?:string;
    companyStatus?:boolean;
    pocContactNumber?:number;
    pocEmailId?:string;
    companyAddressLine1?:string;
    companyAddressLine2?:string;
    companyAddressLine3?:string;
    zipcode?:number;
    companyContactNo?:number;
    companyEmailId?:string;
    companyWebsiteUrl?:string;
    companyBankName?:string;
    gSTno?:number;
    bankAccNo?:number;
    mSMERegistrationNo?:number;
    iFSCCode?:string;
    panNo?:string;
    countryId?:number;
    stateId?:number;
    cityId?:number;
}

export class CompanyMasterDto{
    //companies:MasterCompany[];
    valueId:number;
    companyName:string;
    companyLocation:string;
    status:boolean;
    TotalRecords:number;

    
    constructor(valueId:number,companyName:string,companyLocation:string,status:boolean,TotalRecords:number) {
    this.valueId=valueId;
    this.companyName=companyName;
    this.companyLocation=companyLocation;
    this.status=status;
    this.TotalRecords=TotalRecords;      
    }
}

export class AddCompanyDto{
    //without constructor initializing values using nullable type
    companyName?:string | null;
    valueId?:number | null;
    pompanyName?:string | null;
    pocName?:string | null;
    companyStatus?:boolean | null;
    pocContactNumber?:number | null;
    pocEmailId?:string | null;
    companyAddressLine1?:string | null;
    companyAddressLine2?:string | null;
    companyAddressLine3?:string | null;
    zipcode?:number | null;
    companyContactNo?:number | null;
    companyEmailId?:string | null;
    companyWebsiteUrl?:string | null;
    companyBankName?:string | null;
    gSTno?:number | null;
    bankAccNo?:number | null;
    mSMERegistrationNo?:number | null;
    iFSCCode?:string | null;
    PanNo?:string | null;
    countryId?:number | null;
    stateId?:number | null;
    cityId?:number | null;
}

export class CompanyListResponse {
    companies: CompanyMasterDto[] = [];
    totalCount: number = 0;
  }
  

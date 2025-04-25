export class MasterCompany {
    valueId?:number;
    pompanyName?:string;
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
    PanNo?:string;
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
    valueId?:number;
    pompanyName?:string;
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
    PanNo?:string;
    countryId?:number;
    stateId?:number;
    cityId?:number;
}

export class CompanyListResponse {
    companies: CompanyMasterDto[] = [];
    totalCount: number = 0;
  }
  

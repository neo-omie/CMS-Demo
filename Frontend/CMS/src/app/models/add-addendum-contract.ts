export class AddAddendumContract {
    addendumContractId:number;
    contractId:number;
    contractName:string;
    departmentId:number;
    departmentName:string;
    contractWithCompanyId:number;
    contractWithCompanyName:string;
    contractTypeId:number;
    contractTypeName:string;
    apostilleTypeId:number;
    apostilleTypeName:string;
    actualDocRefNo:number;
    retainerContract:number;
    termsAndConditions:string;
    validFrom:string;
    validTill:string;
    addendumDate:string;
    approver1Status:number;
    approver1Email:string;
    approver2Status:number;
    approver2Email:string;
    approver3Status:number;
    approver3Email:string;
    empCustodianId:number;
    empCustodianName:string;
    location:string;
    isDeleted:boolean;

constructor(
    addendumContractId:number=0,
    contractId:number=0,
    contractName:string='',
    departmentId:number=0,
    departmentName:string='',
    contractWithCompanyId:number=0,
    contractWithCompanyName:string='',
    contractTypeId:number=0,
    contractTypeName:string='',
    apostilleTypeId:number=0,
    apostilleTypeName:string='',
    actualDocRefNo:number=0,
    retainerContract:number=0,
    termsAndConditions:string='',
    validFrom:string='',
    validTill:string='',
    addendumDate:string='',
    approver1status:number=1,
    approver1Email:string = '',
    approver2status:number=1,
    approver2Email:string = '',
    approver3status:number=1,
    approver3Email:string = '',
    empCustodianId:number=0,
    empCustodianName:string='',
    location:string='',
    isDeleted:boolean=false
){
    this.addendumContractId=addendumContractId;
    this.contractId=contractId;
    this.contractName=contractName;
    this.departmentId=departmentId;
    this.departmentName=departmentName;
    this.contractWithCompanyId=contractWithCompanyId;
    this.contractWithCompanyName=contractWithCompanyName;
    this.contractTypeId=contractTypeId;
    this.contractTypeName=contractTypeName;
    this.apostilleTypeId=apostilleTypeId;
    this.apostilleTypeName=apostilleTypeName;
    this.actualDocRefNo=actualDocRefNo;
    this.retainerContract=retainerContract;
    this.termsAndConditions=termsAndConditions;
    this.validFrom=validFrom;
    this.validTill=validTill;
    this.addendumDate=addendumDate;
    this.approver1Status=approver1status;
    this.approver1Email = approver1Email;
    this.approver2Status=approver2status;
    this.approver2Email = approver2Email;
    this.approver3Status=approver3status;
    this.approver3Email = approver3Email;
    this.empCustodianId=empCustodianId;
    this.empCustodianName=empCustodianName;
    this.location=location;
    this.isDeleted=isDeleted;
}
}

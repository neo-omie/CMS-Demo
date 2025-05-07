export class ContractTypeMaster {
    valueId?:number;
    contractTypeName?:string;
    status?:boolean;
}

export class ContractTypeMasterDTO{
valueId:number;
contractTypeName:string;
status:boolean;
totalRecords:number;

/**
 *
 */
constructor(valueId:number,contractTypeName:string,status:boolean,totalRecords:number) {
    this.contractTypeName=contractTypeName;
    this.valueId=valueId;
    this.status=status;
    this.totalRecords=totalRecords;
    
}
}
export class ContractListResponse {
    companies: ContractTypeMasterDTO[] = [];
    totalCount: number = 0;
  }

  export class AddContractDTO{
    contractTypeName?:string | null;
    status?:boolean | null;
  }


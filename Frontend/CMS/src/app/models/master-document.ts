export class MasterDocument {
  // valueId?: number ;
  // documentName?: string  ;
  // status?: number;

  valueId: number;
  documentName: string;
  status: number;

  constructor(valueId:number, documentName: string, status: number) {
    this.valueId=valueId;
    this.status = status;
    this.documentName = documentName;
  }
}
export class MasterDocumentDto {
  documents: MasterDocument[];
  totalCount: number;

  constructor(documents: MasterDocument[], totalCount: number) {
    this.documents = documents;
    this.totalCount = totalCount;
  }
}

export class AddDocumentDto {
  documentName: string;
  status: number;
  constructor(documentName: string, status: number) {
    this.documentName = documentName;
    this.status = status;
  }

  
}

export class GetDocumentById{
  valueId :number;
  documentName : string;
  status:number;
  
  constructor(valueId:number,documentName:string,status:number){
    this.valueId=valueId;
    this.documentName=documentName;
    this.status = status;

  }
}

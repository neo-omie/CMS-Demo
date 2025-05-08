export class MasterDocument {
  // valueId?: number ;
  // documentName?: string  ;
  // status?: number;

  valueId: number;
  displayDocumentName: string;
  status: number;
  documentPath: string;
  // displayDocumentName:string;


  constructor(valueId:number, displayDocumentName: string, status: number, documentPath:string) {
    this.valueId=valueId;
    this.status = status;
    this.displayDocumentName = displayDocumentName;
    this.documentPath = documentPath;
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
  file: Blob | null;
  status: number;
  constructor(file: File | null, status: number) {
    this.file = file;
    this.status = status;
  }

  
}

export class GetDocumentById{
  valueId :number;
  displayDocumentName : string;
  documentPath : string;
  // documentData : string;
  status:number;
  
  constructor(valueId:number,displayDocumentName:string,documentPath:string,documentData:string,status:number){
    this.valueId=valueId;
    this.displayDocumentName=displayDocumentName;
    this.documentPath=documentPath;
    // this.documentData=documentData;
    this.status = status;

  }
}

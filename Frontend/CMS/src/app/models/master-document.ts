export class MasterDocument {
  valueId?: number ;
  documentName?: string  ;
  status?: number;
  // isDeleted:number;
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
  constructor(documentName:string,status:number){
    this.documentName=documentName;
    this.status=status;
  }

  
}

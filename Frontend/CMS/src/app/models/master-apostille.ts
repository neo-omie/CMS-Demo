export class MasterApostille {
    valueId:number;
    apostilleName:string;
    status:boolean;
    isDeleted:boolean;

constructor(
    valueId:number=0,
    apostilleName:string='',
    status:boolean=true,
    isDeleted:boolean=false
){
    this.valueId=valueId;
    this.apostilleName=apostilleName;
    this.status=status;
    this.isDeleted=isDeleted;
}
}

export class MasterApostilleDto{
allApostilles:MasterApostille[];

constructor(allApostilles:MasterApostille[]){
    this.allApostilles=allApostilles;
}
}

export class AddApostilleDto{
    apostilleName?:string;
    status?:boolean;
}


export class EditApostilleDto{
    apostilleName?:string;
    status?:boolean;
}


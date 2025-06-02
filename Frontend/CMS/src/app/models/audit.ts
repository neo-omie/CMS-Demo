export class Audit {

    tableName:number;
    loggedBy:string;
    logTime:Date;
    actionDescription:string;
    statusName:number;
    totalRecords:number ;

    constructor(tableName :number,logBy:string,logTime:Date,actionDesc:string,status:number,totalRecords:number){
        this.actionDescription=actionDesc;
        this.tableName=tableName;
        this.loggedBy=logBy;
        this.logTime=logTime;
        this.statusName=status;
        this.totalRecords =totalRecords;

    }
}

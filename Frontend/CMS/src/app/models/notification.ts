export class Notification {
    valueId: number;
    employeeCode: string;
    notficationSubject: string;
    notficationMessage: string;
    notificationDate: string;
    totalRecords : number;
    isRead?: boolean;
    constructor(valueId:number, employeeCode:string, notficationSubject:string, notficationMessage:string,totalRecords : number, notficationDate:string)
    {
        this.valueId = valueId;
        this.employeeCode = employeeCode;
        this.notficationSubject = notficationSubject
        this.notficationMessage = notficationMessage;
        this.notificationDate = notficationDate;
        this.totalRecords = totalRecords;
    }
}

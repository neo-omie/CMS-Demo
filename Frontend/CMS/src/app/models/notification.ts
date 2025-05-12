export class Notification {
    valueId: number;
    employeeCode: string;
    notficationSubject: string;
    notficationMessage: string;
    notificationDate: string;
    constructor(valueId:number, employeeCode:string, notficationSubject:string, notficationMessage:string, notficationDate:string)
    {
        this.valueId = valueId;
        this.employeeCode = employeeCode;
        this.notficationSubject = notficationSubject
        this.notficationMessage = notficationMessage;
        this.notificationDate = notficationDate;
    }
}

export class NoticeWithdrawal {
    valueId?:number;
}
export class WithdrawNoticeUploadDTO {
    file:Blob | null;
    Remark:string;
    constructor(file:File | null, Remark:string) {
       this.file=file;
       this.Remark=Remark;
    }
}
export class ApproveRejectWithdrawalDTO {
    contractId?: number;
    employeeEmail?: string;
    changeToStatus?: number;
    emailSubject?: string | null;
    emailBody?: string | null;
}
export class ClassifiedApproveRejectWithdrawalDTO {
    classifiedContractId?: number;
    employeeEmail?: string;
    changeToStatus?: number;
    emailSubject?: string | null;
    emailBody?: string | null;
}
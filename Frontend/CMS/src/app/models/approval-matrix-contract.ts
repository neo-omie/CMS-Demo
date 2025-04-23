export class ApprovalMatrixContract {
    masterApprovalMatrixContractId : number;
    departmentName : string;
    departmentId : number;
    approverName1 : string;
    approverId1 : string;
    approverName2 : string;
    approverId2 : string;
    approverName3 : string;
    approverId3 : string;
    numberOfDays : number;
    totalRecords : number;
    constructor(masterApprovalMatrixContractId : number, 
        departmentName : string, 
        departmentId : number, 
        approverName1 : string, 
        approverId1 : string, 
        approverName2 : string, 
        approverId2 : string, 
        approverName3 : string, 
        approverId3 : string, 
        numberOfDays : number,
        totalRecords : number){
        this.masterApprovalMatrixContractId = masterApprovalMatrixContractId;
        this.departmentName = departmentName;
        this.departmentId = departmentId;
        this.approverName1 = approverName1;
        this.approverId1 = approverId1;
        this.approverName2 = approverName2;
        this.approverId2 = approverId2;
        this.approverName3 = approverName3;
        this.approverId3 = approverId3;
        this.numberOfDays = numberOfDays;
        this.totalRecords = totalRecords;
    }
}

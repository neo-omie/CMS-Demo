export class MasterEscalationMatrixMouDto {
    matrixMouId: number;
    escalation1: string;
    escalation2: string;
    escalation3: string;
    escalationId1: string;
    escalationId2: string;
    escalationId3: string;
    departmentId : number;
    departmentName: string;
    triggerDaysEscalation1 :number;
    triggerDaysEscalation2 :number;
    triggerDaysEscalation3 :number;
    totalRecords :number;
  
    constructor(
      matrixMouId: number,
      escalation1: string,
      escalation2: string,
      escalation3: string,
      departmentId:number,
      departmentName: string,
      escalationId1: string,
      escalationId2: string,
      escalationId3: string,
      triggerDaysEscalation1:number,
      triggerDaysEscalation2:number,
      triggerDaysEscalation3:number,
      totalRecords:number
    ) {
      this.departmentId = departmentId;
      this.matrixMouId = matrixMouId;
      this.departmentName = departmentName;
      this.escalation1 = escalation1;
      this.escalation2 = escalation2;
      this.escalation3 = escalation3;
      this.escalationId1 = escalationId1;
      this.escalationId2 = escalationId2;
      this.escalationId3 = escalationId3;
      this.triggerDaysEscalation1 = triggerDaysEscalation1;
      this.triggerDaysEscalation2 = triggerDaysEscalation2;
      this.triggerDaysEscalation3 = triggerDaysEscalation3;
      this.totalRecords = totalRecords;
    }
  }
  
  export class UpdateMatrixMouDto{
    escalationId1: string;
    escalationId2: string;
    escalationId3: string;
    triggerDaysEscalation1 :number;
    triggerDaysEscalation2 :number;
    triggerDaysEscalation3 :number;
    constructor(
      matrixContractId: number,
      escalationId1: string,
      escalationId2: string,
      escalationId3: string,
      departmentName: string,
      triggerDaysEscalation1:number,
      triggerDaysEscalation2:number,
      triggerDaysEscalation3:number
  
  
    ) {
      this.escalationId1 = escalationId1;
      this.escalationId2 = escalationId2;
      this.escalationId3 = escalationId3;
      this.triggerDaysEscalation1 = triggerDaysEscalation1;
      this.triggerDaysEscalation2 = triggerDaysEscalation2;
      this.triggerDaysEscalation3 = triggerDaysEscalation3;
    }
  }
  
  
  export class GetMasterEscalationMatrixContractByIdDto {
    matrixContractId: number;
    escalation1: string;
    escalation2: string;
    escalation3: string;
    departmentName: string;
    triggerDaysEscalation1 :number;
    triggerDaysEscalation2 :number;
    triggerDaysEscalation3 :number;
  
    constructor(
      matrixContractId: number,
      escalation1: string,
      escalation2: string,
      escalation3: string,
      departmentName: string,
      triggerDaysEscalation1:number,
      triggerDaysEscalation2:number,
      triggerDaysEscalation3:number
  
  
    ) {
      this.matrixContractId = matrixContractId;
      this.departmentName = departmentName;
      this.escalation1 = escalation1;
      this.escalation2 = escalation2;
      this.escalation3 = escalation3;
      this.triggerDaysEscalation1 = triggerDaysEscalation1;
      this.triggerDaysEscalation2 = triggerDaysEscalation2;
      this.triggerDaysEscalation3 = triggerDaysEscalation3;
    }
  }
  

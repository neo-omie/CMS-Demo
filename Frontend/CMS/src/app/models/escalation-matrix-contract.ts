export class EscalationMatrixContract {
  matrixContractId: number;
  escalation1: string;
  escalation2: string;
  escalation3: string;
  departmentName: string;

  constructor(
    matrixContractId: number,
    escalation1: string,
    escalation2: string,
    escalation3: string,
    departmentName: string
  ) {
    this.matrixContractId = matrixContractId;
    this.departmentName = departmentName;
    this.escalation1 = escalation1;
    this.escalation2 = escalation2;
    this.escalation3 = escalation3;
  }
}

export class MasterEscalationMatrixContractDto {
  matrixContract: EscalationMatrixContract[];
  totalCount: number;

  constructor(matrixContract: EscalationMatrixContract[], totalCount: number) {
    
    this.matrixContract=matrixContract;
    this.totalCount=totalCount;
  }

}

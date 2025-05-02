export class ContractsEntity {
    contractID: number;
    contractName: string;
    contractType: string;
    departmentName: string;
    effectiveDate: Date;
    expiryDate: Date;
    toBeRenewedOn: Date;
    addendumDate: Date;
    status: number;
    approvalPendingFrom: string;
    renewalContractPerson: string;
    renewalDueIn: string;
    location: string;
    totalRecords: number;
    constructor(
        contractID: number,
        contractName: string,
        contractType: string,
        departmentName: string,
        effectiveDate: Date,
        expiryDate: Date,
        toBeRenewedOn: Date,
        addendumDate: Date,
        status: number,
        approvalPendingFrom: string,
        renewalContractPerson: string,
        renewalDueIn: string,
        location: string,
        totalRecords: number
    ) {
        this.contractID = contractID;
        this.contractName = contractName;
        this.contractType = contractType;
        this.departmentName = departmentName;
        this.effectiveDate = effectiveDate;
        this.expiryDate = expiryDate;
        this.toBeRenewedOn = toBeRenewedOn;
        this.addendumDate = addendumDate;
        this.status = status;
        this.approvalPendingFrom = approvalPendingFrom;
        this.renewalContractPerson = renewalContractPerson;
        this.renewalDueIn = renewalDueIn;
        this.location = location;
        this.totalRecords = totalRecords;
    }
}
export class GetContractByIdDto {
    contractId?: number;
    contractName?: string;
    departmentId?: number;
    departmentName?: string;
    contractWithCompanyId?: number;
    contractWithCompanyName?: string;
    contractTypeId?: number;
    contractTypeName?: string;
    apostilleTypeId?: number;
    apostilleTypeName?: string;
    actualDocRefNo?: number;
    retainerContract?: number;
    termsAndConditions?: string;
    validFrom?: Date;
    validTill?: Date;
    renewalFrom?: Date;
    renewalTill?: Date;
    addendumDate?: Date;
    empCustodianId?: number;
    empCustodianName?: string;
    location?: string;
    approver1Status?: number;
    approver2Status?: number;
    approver3Status?: number;
    isDeleted?: boolean;
}
export class AddContractDto {
    contractName?: string;
  departmentId?: number;
  contractWithCompanyId?: number;
  contractTypeId?: number;
  apostilleTypeId?: number;
  actualDocRefNo?: number;
  retainerContract?: number;
  termsAndConditions?: string;
  validFrom?: Date;
  validTill?: Date;
  renewalFrom?: Date;
  renewalTill?: Date;
  addendumDate?: Date;
  empCustodianId?: number;
  location?: string;
  approver1Status?: number;
  approver2Status?: number;
  approver3Status?: number;
  isDeleted?: true;
}
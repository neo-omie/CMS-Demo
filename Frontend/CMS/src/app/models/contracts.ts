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
    approver1Email?:string;
    approver2Status?: number;
    approver2Email?:string;
    approver3Status?: number;
    approver3Email?:string;
    isDeleted?: boolean;
}
export class AddContractDto {
    contractName?: string | null;
    departmentId?: number | null;
    contractWithCompanyId?: number | null;
    contractTypeId?: number | null;
    apostilleTypeId?: number | null;
    actualDocRefNo?: number | null;
    retainerContract?: number | null;
    termsAndConditions?: string | null;
    validFrom?: string | null;
    validTill?: string | null;
    renewalFrom?: string | null;
    renewalTill?: string | null;
    addendumDate?: string | null;
    empCustodianId?: number | null;
    location?: string | null;
    approver1Status?: number | null;
    approver2Status?: number | null;
    approver3Status?: number | null;
    isDeleted: boolean = false;
}
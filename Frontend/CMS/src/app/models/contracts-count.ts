export class ContractsCount {
    allContractsCount: number;
    pendingApprovalContractsCount: number;
    pendingTerminationContractsCount: number;
    expiredContractsCount: number;
    activeContractsCount: number;
    terminatedContractsCount: number;
    constructor(
        allContractsCount: number,
        pendingApprovalContractsCount: number,
        pendingTerminationContractsCount: number,
        expiredContractsCount: number,
        activeContractsCount: number,
        terminatedContractsCount: number,
    ) {
        this.allContractsCount = allContractsCount;
        this.pendingApprovalContractsCount = pendingApprovalContractsCount;
        this.pendingTerminationContractsCount = pendingTerminationContractsCount;
        this.expiredContractsCount = expiredContractsCount;
        this.activeContractsCount = activeContractsCount;
        this.terminatedContractsCount = terminatedContractsCount;
    }
}

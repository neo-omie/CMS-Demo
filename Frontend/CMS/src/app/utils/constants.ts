export enum ContractStatus {
    PendingApproval = 1, 
    Active, 
    Rejected, 
    Terminated, 
    Expired, 
    PendingTermination, 
    ApprovedForTermination, 
    PendingNoticeWithdrawn
}

export enum Location {
    Thane = 'Thane', 
    Indore = 'Indore', 
    Pune = 'Pune',
    Mumbai = 'Mumbai',
    Banglore = 'Banglore'
}

export const MY_DATE_FORMATS = {
  parse: {
    dateInput: 'DD-MM-YYYY',
  },
  display: {
    dateInput: 'dd-MM-yyyy',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'dd-MM-yyyy',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};
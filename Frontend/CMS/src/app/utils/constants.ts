export enum ContractStatus {
    Pending_Approval = 1, 
    Active, 
    Rejected, 
    Terminated, 
    Expired, 
    Pending_Termination, 
    Approved_For_Termination, 
    Pending_Notice_Withdrawn
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
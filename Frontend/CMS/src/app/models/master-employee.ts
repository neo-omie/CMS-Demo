export class MasterEmployee {
    valueId : number;
    email : string;
    role : string;
    employeeName : string;
    employeeCode : string;
    unit : string;
    employeeMobile : string;
    employeeExtension : string;
    departmentId : number;
    constructor(
        valueId : number,
        email : string,
        role : string,
        employeeName : string,
        employeeCode : string,
        unit : string,
        employeeMobile : string,
        employeeExtension : string,
        departmentId : number
    ){
        this.valueId = valueId;
        this.email = email;
        this.role  = role;
        this.employeeName = employeeName;
        this.employeeCode = employeeCode;
        this.unit = unit;
        this.employeeMobile = employeeMobile;
        this.employeeExtension = employeeExtension;
        this.departmentId = departmentId;
    }
}

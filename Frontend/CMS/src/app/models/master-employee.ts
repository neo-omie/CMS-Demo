export class MasterEmployee {
    valueId:number;
    email:string;
    role:string;
    employeeName:string;
    isDeleted:boolean;
    employeeCode: string;
    unit: string;
    employeeMobile: number;
    employeeExtension: string;
    departmentId: number;
    lastPasswordChanged: Date;
    departmentName:string;

constructor(
    valueId: number=0,
    email:string='',
    role:string='',
    employeeName:string='',
    isDeleted:boolean=false,
    employeeCode: string='',
    unit: string='',
    employeeMobile: number=0,
    employeeExtension: string='',
    departmentId: number=0,
    lastPasswordChanged: Date=new Date(),
    departmentName:string
){
    this.valueId = valueId;
    this.email = email;
    this.role = role;
    this.employeeName = employeeName;
    this.isDeleted = isDeleted;
    this.employeeCode = employeeCode;
    this.unit = unit;
    this.employeeMobile = employeeMobile;
    this.employeeExtension = employeeExtension;
    this.departmentId = departmentId;
    this.lastPasswordChanged = lastPasswordChanged;
    this.departmentName = departmentName;
}
}

export class MasterEmployeeDto {
  data: MasterEmployee[];
  totalCount: number;

  constructor(data: MasterEmployee[], totalCount: number) {
    this.data = data;
    this.totalCount = totalCount;
  }
}

export class AddEmployeeDto{
  employeeName?:string
      password?:string
      role?:string
      employeeCode?:string
      unit?:string
      departmentId?:number
      employeeMobile?:number
      email?:string
      employeeExtension?:string
}

export class EditEmployeeDto{
  employeeName?:string
      password?:string
      role?:string
      employeeCode?:string
      unit?:string
      departmentId?:number
      employeeMobile?:number
      email?:string
      employeeExtension?:string
}
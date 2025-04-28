export class MasterDepartment {
    departmentId:number;
    departmentName:string;
    constructor(departmentId:number, departmentName:string) {
        this.departmentId = departmentId;
        this.departmentName = departmentName;
    }
}
export class GetAllDepartmentsDto {
    departmentId:number;
    departmentName:string;
    totalRecords:number;
    constructor(departmentId:number, departmentName:string, totalRecords:number) {
        this.departmentId = departmentId;
        this.departmentName = departmentName;
        this.totalRecords = totalRecords;
    }
}
export class AddDepartmentDto {
    departmentName:string;
    constructor(departmentName:string) {
        this.departmentName = departmentName;
    }
}

export class JwtClaims {
    sub: string; // Employee Name
    email: string; // Employee Email
    ECode: string; // Employee Code
    ERole: string; // Employee Role
    constructor(sub:string, email:string, ECode:string, ERole:string) {
        this.sub = sub;
        this.email = email;
        this.ECode = ECode;
        this.ERole = ERole;
    }
}

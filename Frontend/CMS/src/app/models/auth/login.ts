export class Login {
    email?:string | null;
    password?:string | null;
    constructor(email:string, password:string)
    {
        this.email = email;
        this.password = password;
    }
}
export class AuthResponse {
    token:string;
    // userId:string;
    // email:string;
    // name:string;
    // role:string;
    constructor(token:string/*, userId:string, email:string, name:string, role:string*/)
    {
        this.token = token;
        // this.userId = userId;
        // this.email = email;
        // this.name = name;
        // this.role = role;
    }
}

export class PasswordRenewal {
    email:string;
    oldPassword:string;
    newPassword:string;
    reenterNewPassword:string;
    constructor(email:string, oldPassword:string, newPassword:string, reenterNewPassword:string)
    {
        this.email = email;
        this.oldPassword = oldPassword;
        this.newPassword = newPassword;
        this.reenterNewPassword = reenterNewPassword;
    }
}
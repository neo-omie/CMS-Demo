export class Login {
    email:string;
    password:string;
    constructor(email:string, password:string)
    {
        this.email = email;
        this.password = password;
    }
}
export class AuthResponse {
    userId:string;
    email:string;
    name:string;
    token:string;
    role:string;
    constructor(userId:string, email:string, name:string, token:string, role:string)
    {
        this.userId = userId;
        this.email = email;
        this.name = name;
        this.token = token;
        this.role = role;
    }
}
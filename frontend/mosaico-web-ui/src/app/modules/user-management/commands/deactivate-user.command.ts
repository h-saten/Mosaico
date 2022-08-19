export interface DeactivateUserCommand {
    id:string;
    status:boolean;
    reason?:string;
}

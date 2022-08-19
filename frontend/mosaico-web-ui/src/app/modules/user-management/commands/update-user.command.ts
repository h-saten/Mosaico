export interface UpdateUserCommand {
    firstName: string | null;
    lastName: string | null;
    timezone: string | null;
    country : string | null;
    postalCode:string | null;
    city:string | null;
    street:string | null;
    dob:Date | null;
}

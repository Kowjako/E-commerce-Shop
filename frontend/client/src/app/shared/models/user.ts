export interface User {
    email: string;
    displayName: string;
    jwtToken: string;
}

export interface Address {
    firstName: string;
    lastName: string;
    street: string;
    city: string;
    state: string;
    zipcode: string;
}
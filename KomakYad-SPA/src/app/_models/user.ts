export interface User {
    id: number;
    username: string;
    email: string;
    firstName: string;
    lastName: string;
    createdOn: Date;
    locked: boolean;
    cardLimit?: number;
    collectionLimit?: number;
}

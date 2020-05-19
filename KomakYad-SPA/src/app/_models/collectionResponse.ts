import { User } from './user';

export interface CollectionResponse {
    id: number;
    uniqueId: string;
    author: User;
    title: string;
    description: string;
}

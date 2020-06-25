import { User } from './user';

export interface CollectionResponse {
    id: number;
    uniqueId: string;
    author: User;
    title: string;
    description: string;
    isPrivate: boolean;
    cardsCount: number;
    inReadCount: number;
}

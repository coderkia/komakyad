import { CollectionResponse } from './collectionResponse';

export interface CardResponse {
    id: number;
    uniqueId: string;
    answer: string;
    question: string;
    example: string;
    extraData: any;
    collection: CollectionResponse;
}

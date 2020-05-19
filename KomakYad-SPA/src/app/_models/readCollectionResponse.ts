import { CollectionResponse } from './collectionResponse';

export interface ReadCollectionResponse {
    id: number;
    isReversed: boolean;
    priority: number;
    collection: CollectionResponse;
    readPerDay: number;
    deleted: boolean;
}

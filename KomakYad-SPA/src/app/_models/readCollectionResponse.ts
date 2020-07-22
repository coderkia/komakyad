import { CollectionResponse } from './collectionResponse';
import { ReadOverview } from './readOverview';

export interface ReadCollectionResponse {
    id: number;
    isReversed: boolean;
    priority: number;
    collection: CollectionResponse;
    readPerDay: number;
    deleted: boolean;
    overview?: ReadOverview;
}

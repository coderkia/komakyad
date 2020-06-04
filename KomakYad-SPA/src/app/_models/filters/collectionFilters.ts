export interface CollectionFilter {
    maxPageSize?: number;
    orderBy?: string;
    orderByDesc?: string;
    pageSize?: number;
    pageNumber?: number;
    authorId?: number;
    title?: string;
    includePrivateCollections?: boolean;
}

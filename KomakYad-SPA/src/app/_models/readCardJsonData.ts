import { TextStyle } from './textStyle';

export interface ReadCardJsonData {
    style?: {
        question?: TextStyle;
        answer?: TextStyle;
        example?: TextStyle;
    };
}

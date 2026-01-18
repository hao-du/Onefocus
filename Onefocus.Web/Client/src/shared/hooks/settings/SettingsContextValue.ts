import { QueryObserverResult } from '@tanstack/react-query';
import GetSettingsByUserIdResponse from '../../apis/interfaces/GetSettingsByUserIdResponse';

export default interface SettingsContextValue {
    settings?: GetSettingsByUserIdResponse;
    refetch: () => Promise<QueryObserverResult<GetSettingsByUserIdResponse, Error>>;
    isSettingsReady?: boolean;
}
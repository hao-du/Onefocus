import GetSettingsByUserIdResponse from '../../apis/interfaces/GetSettingsByUserIdResponse';

export default interface SettingsContextValue {
    settings?: GetSettingsByUserIdResponse;
    refetch: () => void;
    isSettingsReady?: boolean;
}
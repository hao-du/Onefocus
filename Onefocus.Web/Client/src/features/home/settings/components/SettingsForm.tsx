import { useForm } from 'react-hook-form';
import SettingFormInput from './interfaces/SettingFormInput';
import { Workspace } from '../../../../shared/components/layouts/workspace';
import { useWindows } from '../../../../shared/components/hooks';
import useUpsertSetting from '../services/useUpsertSetting';
import { useMemo } from 'react';
import useGetAllLocales from '../services/useGetAllLocales';
import useGetAllTimeZones from '../services/useGetAllTimeZones';
import { Dropdown, Option } from '../../../../shared/components/controls';
import { getEmptyGuid } from '../../../../shared/utils/formatUtils';
import useGetSettingsByUserId from '../services/useGetSettingByUserId';
import { useSettings } from '../../../../shared/hooks';

const SettingsForm = () => {
    const { showResponseToast } = useWindows();
    const { refetch } = useSettings();
    const { entity: allLocales, isEntityLoading: isAllLocalesLoading } = useGetAllLocales();
    const { entity: allTimeZones, isEntityLoading: isAllTimeZonesLoading } = useGetAllTimeZones();
    const { entity: setting, isEntityLoading: isSettingLoading } = useGetSettingsByUserId();
    const { onUpsertAsync, isUpserting } = useUpsertSetting();

    const isPending = isSettingLoading || isUpserting || isAllLocalesLoading || isAllTimeZonesLoading;

    const formValues = useMemo(() => {
        return setting ??
        {
            locale: '',
            timeZone: ''
        }
    }, [setting]);

    const { control, handleSubmit } = useForm<SettingFormInput>({
        defaultValues: formValues,
        values: formValues
    });
    
    return (
        <Workspace
            title="Settings"
            isPending={isPending}
            actionItems={[
                {
                    label: 'Save',
                    icon: 'pi pi-save',
                    command: () => {
                        handleSubmit(async (data: SettingFormInput) => {
                            const response = await onUpsertAsync({
                                locale: data.locale,
                                timeZone: data.timeZone
                            });
                            await refetch();
                            showResponseToast(response, 'Saved successfully.');
                        })();
                    }
                }
            ]}
            leftPanel={
                <div className="overflow-auto flex-1">
                    <form>
                        <Dropdown control={control}
                            name="locale"
                            label="Locale"
                            className="w-full of-w-max"
                            options={allLocales?.map(locale => ({
                                value: locale.code,
                                label: `${locale.nativeName}`
                            }) as Option) ?? []}
                            rules={{
                                validate: (value) => value && value !== getEmptyGuid() ? true : 'Locale is required.'
                            }} />
                        <Dropdown control={control}
                            name="timeZone"
                            label="Timezone"
                            className="w-full of-w-max"
                            options={allTimeZones?.map(timeZone => ({
                                value: timeZone.id,
                                label: `${timeZone.displayName}`
                            }) as Option) ?? []}
                            rules={{
                                validate: (value) => value && value !== getEmptyGuid() ? true : 'Timezone is required.'
                            }} />
                    </form>
                </div>
            }
        />
    );
};

export default SettingsForm;
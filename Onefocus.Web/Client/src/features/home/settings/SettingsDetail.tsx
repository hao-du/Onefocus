import { useEffect, useMemo } from "react";
import { useForm } from "react-hook-form";
import useSettings from "../../../shared/hooks/settings/useSettings";
import useWindows from "../../../shared/hooks/windows/useWindows";
import useGetAllLocales from "./services/useGetAllLocales";
import useGetAllTimeZones from "./services/useGetAllTimeZones";
import useGetSettingsByUserId from "./services/useGetSettingByUserId";
import useUpsertSettings from "./services/useUpsertSetting";
import usePage from "../../../shared/hooks/page/usePage";
import Icon from "../../../shared/components/atoms/misc/Icon";
import { ActionOption } from "../../../shared/options/ActionOption";
import DefaultLayout from "../../../shared/components/layouts/DefaultLayout";
import Card from "../../../shared/components/molecules/panels/Card";
import FormSelect from "../../../shared/components/molecules/forms/FormSelect";
import { getEmptyGuid } from "../../../shared/utils";
import Form from "../../../shared/components/molecules/forms/Form";

interface SettingFormInput {
    locale: string;
    timeZone: string;
}

const SettingsDetail = () => {
    const { setLoadings, hasAnyLoading } = usePage();
    const { showResponseToast } = useWindows();
    const { refetch } = useSettings();
    const { locales, isLocalesLoading: isLocaleLoading } = useGetAllLocales();
    const { timezones, isTimeZonesLoading: isTimeZoneLoading } = useGetAllTimeZones();
    const { userSettings, isUserSettingsLoading: isUserSettingLoading } = useGetSettingsByUserId();
    const { upsertAsync, isUpserting } = useUpsertSettings();

    const formValues = useMemo(() => {
        return userSettings ??
        {
            locale: '',
            timeZone: ''
        }
    }, [userSettings]);

    const { control, handleSubmit } = useForm<SettingFormInput>({
        defaultValues: formValues,
        values: formValues
    });

    const onSave = handleSubmit(async (data) => {
        const response = await upsertAsync({
            locale: data.locale,
            timeZone: data.timeZone
        });
        await refetch();
        showResponseToast(response, 'Saved successfully.');
    });

    const actions = useMemo<ActionOption[]>(() => [
        {
            id: 'btnAdd',
            icon: <Icon name="save" />,
            label: 'Save',
            isPending: hasAnyLoading,
            command: onSave,
        }
    ], [hasAnyLoading, onSave]);

    useEffect(() => {
        setLoadings({
            isUserSettingLoading, isUpserting, isLocaleLoading, isTimeZoneLoading
        });
    }, [isTimeZoneLoading, isLocaleLoading, isUserSettingLoading, isUpserting, setLoadings]);

    return (
        <DefaultLayout
            title="Settings"
            showPrimaryButton
            actions={actions}
        >
            <Card
                body={
                    <Form
                        className="w-full lg:w-md"
                    >
                        <FormSelect control={control}
                            name="locale"
                            label="Locale"
                            options={locales?.map(locale => ({
                                value: locale.code,
                                label: `${locale.nativeName}`
                            })) ?? []}
                            rules={{
                                validate: (value) => value && value !== getEmptyGuid() ? true : 'Locale is required.'
                            }} />
                        <FormSelect control={control}
                            name="timeZone"
                            label="Timezone"
                            options={timezones?.map(timeZone => ({
                                value: timeZone.id,
                                label: `${timeZone.displayName}`
                            })) ?? []}
                            rules={{
                                validate: (value) => value && value !== getEmptyGuid() ? true : 'Timezone is required.'
                            }} />
                    </Form>
                }
            >
            </Card >
        </DefaultLayout >
    );
}

export default SettingsDetail;

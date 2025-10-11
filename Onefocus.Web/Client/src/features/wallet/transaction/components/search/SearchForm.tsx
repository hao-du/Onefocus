import { useForm } from 'react-hook-form';
import { TextOnly } from '../../../../../shared/components/controls';
import { Text } from '../../../../../shared/components/controls';
import { WorkspaceRightPanel } from '../../../../../shared/components/layouts/workspace';
import TransactionSearchFormInput from './interfaces/TransactionSearchFormInput';
import { SearchCriteria } from '../../services';
import { Dispatch, SetStateAction, useEffect } from 'react';

type SearchFormProps = {
    onSearch: (data: TransactionSearchFormInput) => void;
    isPending?: boolean;
    searchCriteria: SearchCriteria;
    setSearchCriteria: Dispatch<SetStateAction<SearchCriteria>>;
}

const SearchForm = (props: SearchFormProps) => {
    const form = useForm<TransactionSearchFormInput>({
        defaultValues: {
            ...props.searchCriteria,
        },
    });

    useEffect(() => {
        //Watch changes and keep it in searchCriteria
        const subscription = form.watch((value, { name }) => {
            if (!name) return;
            props.setSearchCriteria((prev) => ({ ...prev, [name]: value[name] }));
        });

        return () => subscription.unsubscribe();
    }, [form, form.watch, props])

    const buttons = [
        {
            id: 'btnSearch',
            label: 'Search',
            icon: 'pi pi-search',
            onClick: () => {
                form.handleSubmit(props.onSearch)();
            }
        }
    ];

    return (
        <WorkspaceRightPanel buttons={buttons} isPending={props.isPending}>
            <h3 className="mt-0 mb-5">
                <TextOnly value="Search" />
            </h3>
            <form>
                <Text
                    control={form.control}
                    name="text"
                    className="w-full of-w-max"
                    label="Find something..."
                    placeholder="Find something..."
                    rules={{
                        maxLength: { value: 255, message: 'Searchbox cannot exceed 255 characters.' },
                    }}
                />
            </form>
        </WorkspaceRightPanel>
    );
};

export default SearchForm;
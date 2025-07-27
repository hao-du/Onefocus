import React from 'react';
import BankList from '../components/BankList';

const Bank = React.memo(() => {
    return <BankList />;
});

export default Bank;
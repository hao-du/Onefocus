import { ReactNode, useMemo } from 'react';
import { Card as AntCard, Row } from 'antd';
import { ClassNameProps } from '../../../props/BaseProps';
import CardTitle from '../../atoms/typography/CardTitle';
import { HAlignType } from '../../../types';
import useTheme from '../../../hooks/theme/useTheme';
import { GRID_COLUMNS } from '../../../constants';
import Col from '../../atoms/panels/Col';
import { joinClassNames } from '../../../utils';

interface CardProps extends ClassNameProps {
    title?: string;
    titleAlign?: HAlignType
    titleMargin?: number;
    body?: ReactNode;
    bodyStyle?: React.CSSProperties;
    leftActions?: ReactNode;
    actions?: ReactNode;
    rightActions?: ReactNode;
    actionMargin?: number;
    titleExtra?: ReactNode
}

const Card = (props: CardProps) => {
    const { styles } = useTheme();

    const renderActions = useMemo(() => {
        let placeHolderNumber = 0;
        if (props.actions) placeHolderNumber++;
        if (props.leftActions) placeHolderNumber++;
        if (props.rightActions) placeHolderNumber++;
        if (placeHolderNumber == 0) {
            return null;
        }

        const size = GRID_COLUMNS / placeHolderNumber;
        return (
            <Row style={{ marginTop: props.actionMargin ?? styles.size.margin }}>
                {props.leftActions && (
                    <Col span={size}>
                        {props.leftActions}
                    </Col>
                )}
                {props.actions && (
                    <Col span={size} className="text-center">
                        {props.actions}
                    </Col>
                )}
                {props.rightActions && (
                    <Col span={size} className="text-right">
                        {props.rightActions}
                    </Col>
                )}

            </Row>
        )
    }, [props.actionMargin, props.actions, props.leftActions, props.rightActions, styles.size.margin]);

    return (
        <AntCard
            className={joinClassNames(props.className, 'w-full')}
            styles={{
                body: {
                    padding: styles.size.padding,
                    ...props.bodyStyle
                }
            }}
        >

            {props.title && (
                <Row>
                    <Col span={GRID_COLUMNS / 2}>
                        <CardTitle title={props.title} align={props.titleAlign ?? 'left'} style={{ marginBottom: props.titleMargin ?? styles.size.margin }} />
                    </Col>
                    <Col span={GRID_COLUMNS / 2} className='text-right'>
                        {props.titleExtra}
                    </Col>
                </Row>
            )}
            {props.body}
            {renderActions}
        </AntCard>
    );
};

export default Card;
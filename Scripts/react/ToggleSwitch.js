import PropTypes from 'prop-types';
import classnames from 'classnames';
import isString from 'lodash/isString';
import React, { Component } from 'react';
import isBoolean from 'lodash/isBoolean';
import isFunction from 'lodash/isFunction';
import 'Content/bootstrap.min.css';

class ToggleSwitch extends Component {
    constructor() {
        this.state = { enabled: this.enabledFromProps() };
        this.isEnabled = () => this.state.enabled;
        toggleSwitch = evt => {
            evt.persist();
            evt.preventDefault();

            const { onClick, onStateChanged } = this.props;

            this.setState({ enabled: !this.state.enabled }, () => {
                const state = this.state;

                // Augument the event object with SWITCH_STATE
                const switchEvent = Object.assign(evt, { SWITCH_STATE: state });

                // Execute the callback functions
                isFunction(onClick) && onClick(switchEvent);
                isFunction(onStateChanged) && onStateChanged(state);
            });

        }
    }


    enabledFromProps() {
        let { enabled } = this.props;

        // If enabled is a function, invoke the function
        enabled = isFunction(enabled) ? enabled() : enabled;

        // Return enabled if it is a boolean, otherwise false
        return isBoolean(enabled) && enabled;
    }

    render() {
        const { enabled } = this.state;

        // Isolate special props and store the remaining as restProps
        const { enabled: _enabled, theme, onClick, className, onStateChanged, ...restProps } = this.props;

        // Use default as a fallback theme if valid theme is not passed
        const switchTheme = (theme && isString(theme)) ? theme : 'default';

        const switchClasses = classnames(
            `switch switch--${switchTheme}`,
            className
        );

        const togglerClasses = classnames(
            'switch-toggle',
            `switch-toggle--${enabled ? 'on' : 'off'}`
        );

        return (
            <div className={switchClasses} onClick={this.toggleSwitch} {...restProps}>
                <div className={togglerClasses}></div>
            </div>
        );
    }
}

ToggleSwitch.propTypes = {
    enabled: PropTypes.oneOfType([
        PropTypes.bool,
        PropTypes.func
    ]),
    onStateChanged: PropTypes.func,
    theme: PropTypes.string
};

export default ToggleSwitch;
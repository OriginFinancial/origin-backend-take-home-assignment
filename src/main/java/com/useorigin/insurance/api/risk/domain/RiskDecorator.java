package com.useorigin.insurance.api.risk.domain;

public abstract class RiskDecorator implements Risk {

    protected Risk risk;

    public RiskDecorator() {
    }

    public RiskDecorator(Risk risk) {
        this.risk = risk;
    }

}

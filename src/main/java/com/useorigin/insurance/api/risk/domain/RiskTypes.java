package com.useorigin.insurance.api.risk.domain;

import java.util.List;

public class RiskTypes {

    private RiskDecorator noIncome;
    private RiskDecorator noHouse;
    private RiskDecorator noVehicle;
    private RiskDecorator over60;
    private RiskDecorator under30;
    private RiskDecorator between30To40;
    private RiskDecorator withDependents;
    private RiskDecorator isMarried;
    private RiskDecorator incomeAbove200K;
    private RiskDecorator houseMortgaged;
    private RiskDecorator newVehicle;

    //    Risk noAssets = new RiskScoreNoIncome(new RiskScoreNoHouse(new RiskScoreNoVehicle()));
//    Risk ages = new RiskOver60(new RiskUnder30(new RiskBetween30To40()));
//    Risk family = new RiskWithDependents(new RiskIsMarried());
//    Risk assets = new RiskIncomeAbove200K(new RiskHouseMortgaged(new RiskNewCar()));
//
//
    public RiskTypes(RiskDecorator noIncome, RiskDecorator noHouse, RiskDecorator noVehicle, RiskDecorator over60, RiskDecorator under30, RiskDecorator between30To40, RiskDecorator withDependents, RiskDecorator isMarried, RiskDecorator incomeAbove200K, RiskDecorator houseMortgaged, RiskDecorator newVehicle) {
        this.noIncome = noIncome;
        this.noHouse = noHouse;
        this.noVehicle = noVehicle;
        this.over60 = over60;
        this.under30 = under30;
        this.between30To40 = between30To40;
        this.withDependents = withDependents;
        this.isMarried = isMarried;
        this.incomeAbove200K = incomeAbove200K;
        this.houseMortgaged = houseMortgaged;
        this.newVehicle = newVehicle;
    }

    public List<RiskDecorator> allRisks() {
        return List.of(noIncome, noHouse, noVehicle, over60, under30, between30To40, withDependents, isMarried, incomeAbove200K, houseMortgaged, newVehicle);
    }

    public static class Builder {
        private RiskDecorator noIncome;
        private RiskDecorator noHouse;
        private RiskDecorator noVehicle;
        private RiskDecorator over60;
        private RiskDecorator under30;
        private RiskDecorator between30To40;
        private RiskDecorator withDependents;
        private RiskDecorator isMarried;
        private RiskDecorator incomeAbove200K;
        private RiskDecorator houseMortgaged;
        private RiskDecorator newVehicle;

        public Builder hasNoIncome(RiskDecorator noIncome) {
            this.noIncome = noIncome;
            return this;
        }

        public Builder hasNoHouse(RiskDecorator noHouse) {
            this.noHouse = noHouse;
            return this;
        }

        public Builder hasNoVehicle(RiskDecorator noVehicle) {
            this.noVehicle = noVehicle;
            return this;
        }

        public Builder IsUnder30(RiskDecorator under30) {
            this.under30 = under30;
            return this;
        }

        public RiskTypes build() {
            return new RiskTypes(noIncome, noHouse, noVehicle, over60, under30, between30To40, withDependents, isMarried, incomeAbove200K, houseMortgaged, newVehicle);
        }
    }
}

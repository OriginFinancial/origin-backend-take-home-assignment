package com.useorigin.insurance.api.risk.domain;

public class House {

    private final OwnershipStatus ownershipStatus;

    public House(OwnershipStatus ownershipStatus) {
        this.ownershipStatus = ownershipStatus;
    }

    public OwnershipStatus getOwnershipStatus() {
        return ownershipStatus;
    }

    @Override
    public String toString() {
        return "House{" +
                "ownershipStatus=" + ownershipStatus +
                '}';
    }
}

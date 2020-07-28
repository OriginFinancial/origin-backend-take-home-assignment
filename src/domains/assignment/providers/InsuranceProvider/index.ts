import { container } from 'tsyringe';
import IInsuranceProvider from './contracts/IInsuranceProvider';
import InsuranceProvider from './implementations/InsuranceProvider';
import FakeInsuranceProvider from './fake/FakeInsuranceProvider';

const providers = {
  insurance: InsuranceProvider,
  fake: FakeInsuranceProvider,
};

const insuranceProvider =
  process.env.NODE_ENV === 'test' ? providers.fake : providers.insurance;

container.registerSingleton<IInsuranceProvider>(
  'insuranceProvider',
  insuranceProvider,
);

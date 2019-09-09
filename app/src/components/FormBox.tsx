import styled from 'styled-components'
import { flexbox, space, FlexboxProps, SpaceProps } from 'styled-system'

const FormBox = styled.form<FlexboxProps & SpaceProps>`
  display: flex;

  ${flexbox}
  ${space}
`

export default FormBox

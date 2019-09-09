import styled from 'styled-components'
import { flexbox, space, FlexboxProps, SpaceProps } from 'styled-system'

const Button = styled.button<FlexboxProps & SpaceProps>`
  ${flexbox}
  ${space}

  height: 30px;

  background-color: ${props => props.theme.colors.main};
  border: none;
  color: white;
  border-radius: 3px;
  outline: none;
`

export default Button
